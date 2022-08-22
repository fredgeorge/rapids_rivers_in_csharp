/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RapidsRivers.Packets;
using RapidsRivers.Rapids;
using RapidsRivers.Rivers;
using static RapidsRivers.Rivers.River;

namespace RabbitMqBus;

// Understands an attachment to a Rapids using RabbitMQ
// With RabbitMQ in "fanout" mode, you send to an Exchange, which then forwards to each Queue
// So the Exchange serves as the Rapids, and there is one Queue per River
// Note: In this implementation, each Service will have its own River (Rivers not shared)
public class RabbitMqRapidsConnection : RapidsConnection {
    private const int DefaultMaximumReadCount = 9;
    private const string ExchangeName = "rapids";
    private const string RabbitMqPubSub = "fanout";
    private readonly IModel _channel;

    public RabbitMqRapidsConnection(string host, string port) : this(host, int.Parse(port)) { }

    public RabbitMqRapidsConnection(string host, int port) {
        var factory = new ConnectionFactory { HostName = host, Port = port };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(ExchangeName, RabbitMqPubSub, true, true,
            new Dictionary<string, object>()); // Either RabbitMQ finds the exchange, or makes it
    }

    public void Register(PacketListener listener) => Register(listener, river => river.Register(listener));

    public void Register(SystemListener listener) => Register(listener, river => river.Register(listener));

    private void Register(PacketListener listener, Action<River> register ) {
        var river = new River(this, listener.Rules, DefaultMaximumReadCount); // No sharing of Rivers in this implementation
        var riverName = listener.Name; // River/Queue name can be Service name since one River per Service
        register(river);
        ConfigureQueueAsRiver(riverName);
        Console.WriteLine($" [*] [service: {listener.Name}] Waiting for messages. To exit press CTRL+C");
        ConsumeMessages(river, riverName);
    }

    public void Publish(RapidsPacket packet) {
        var body = Encoding.UTF8.GetBytes(packet.ToJsonString());
        _channel.BasicPublish(exchange: ExchangeName, routingKey: "", basicProperties: null, body: body);
    }

    private void ConfigureQueueAsRiver(string riverName) {
        _channel.QueueDeclare(riverName, false, true, true, null);
        _channel.QueueBind(riverName, ExchangeName, "");
    }

    private void ConsumeMessages(River river, string riverName) {
        var consumer = new EventingBasicConsumer(_channel); // Define consumer function triggered on each message
        consumer.Received += (_, ea) => {
            var body = ea.Body.ToArray();
            var jsonString = Encoding.Default.GetString(body);
            river.Message(this, jsonString);
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        _channel.BasicConsume(riverName, false, consumer); // Start consuming
    }
}
