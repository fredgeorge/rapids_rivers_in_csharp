/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RabbitMqBus;
using RapidsRivers.Packets;

namespace NeedService;

public class Need {
    private const string CommunityKey = "community";
    private const string OfferEngineCommunityValue = "offer_engine_family";
    private const string NeedKey = "need";
    private const string CarRentalOfferNeedValue = "car_rental_offer";

    public static void Main(string[] args) // Pass in <IP address> and <port> for RabbitMQ
    {
        if (args.Length != 2)
            throw new ArgumentException(
                "Missing IP and Port arguments! The IP address of the Rapids (as a string), and the Port number of the Rapids (also as a string).");
        var rapidsConnection = new RabbitMqRapidsConnection(args[0], args[1]);
        while (true) {
            var needPacket = Packet.Empty()
                .Set(CommunityKey, OfferEngineCommunityValue)
                .Set(NeedKey, CarRentalOfferNeedValue);
            Console.WriteLine(" [<] {0}", needPacket);
            rapidsConnection.Publish(needPacket);
            Thread.Sleep(5000);
        }
    }
}