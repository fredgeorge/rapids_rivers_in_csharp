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
    private readonly List<Rule> _rules;
    private readonly int _maxReadCount;

    private readonly List<PacketListener> listeners = new();
    private readonly List<SystemListener> systemListeners = new();

    public River(RapidsConnection connection, List<Rule> rules, int maxReadCount) {
        _connection = connection;
        _rules = rules;
        _maxReadCount = maxReadCount;
    }

    public void Message(RapidsConnection connection, string message) {
        throw new NotImplementedException();
    }

    public void register(PacketListener listener) {
        listeners.Add(listener);
    }
    
    public interface PacketListener {
        string name { get; }
        List<Rule> rules { get; }

        bool isStillAlive(RapidsConnection connection);

        void packet(RapidsConnection connection, Packet packet, Status information);

        void rejectedPacket(RapidsConnection connection, Packet packet, Status problems) { }
    }

    public interface SystemListener : PacketListener {
        void invalidFormat(RapidsConnection connection, string invalidString, Status problems);

        void loopDetected(RapidsConnection connection, Packet packet, Status problems);
    }
}