/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RapidsRivers.Packets;
using RapidsRivers.Rapids;
using RapidsRivers.Validation;

namespace RapidsRivers.Rivers;

// Understands a themed flow of messages
public class River : RapidsConnection.MessageListener {
    private readonly RapidsConnection _connection;
    private readonly Rules _rules;
    private readonly int _maxReadCount;

    private readonly List<PacketListener> _listeners = new();
    private readonly List<SystemListener> _systemListeners = new();

    public River(RapidsConnection connection, Rules rules, int maxReadCount) {
        _connection = connection;
        _rules = rules;
        _maxReadCount = maxReadCount;
    }
    
    public void Register(PacketListener listener) {
        _listeners.Add(listener);
    }

    public void Message(RapidsConnection connection, string message) {
        try {
            Packet packet = new(message);
            var status = packet.Evaluate(_rules);
            if (status.HasErrors()) triggerRejectedPacket(connection, packet, status);
            else triggerAcceptedPacket(connection, packet, status);
        }
        catch (PacketException e) {
            new Status(message).Error(e.Message);
        }
    }

    private void triggerAcceptedPacket(RapidsConnection connection, Packet packet, Status problems) => 
        _listeners.ForEach((service) => service.Packet(connection, packet, problems));

    private void triggerRejectedPacket(RapidsConnection connection, Packet packet, Status problems) => 
        _listeners.ForEach((service) => service.RejectedPacket(connection, packet, problems));

    public interface PacketListener {
        string Name { get; }
        Rules Rules { get; }

        bool IsStillAlive(RapidsConnection connection);

        void Packet(RapidsConnection connection, Packet packet, Status information);

        void RejectedPacket(RapidsConnection connection, Packet packet, Status problems) { }
    }

    public interface SystemListener : PacketListener {
        void InvalidFormat(RapidsConnection connection, string invalidString, Status problems);

        void LoopDetected(RapidsConnection connection, Packet packet, Status problems);
    }
}
