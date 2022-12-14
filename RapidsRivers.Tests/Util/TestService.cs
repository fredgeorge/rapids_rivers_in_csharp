/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Collections.Generic;
using RapidsRivers.Packets;
using RapidsRivers.Rapids;
using RapidsRivers.Rivers;
using RapidsRivers.Validation;

namespace RapidsRivers.Tests.Util;

internal class TestService : River.PacketListener {
    public string Name => $"TestService [{GetHashCode()}]";
    public Rules Rules { get; }
    internal readonly List<Packet> AcceptedPackets = new();
    internal readonly List<Packet> RejectedPackets = new();
    internal readonly List<Status> Informations = new();
    internal readonly List<Status> Problems = new();

    internal TestService(Rules rules) {
        Rules = rules;
    }

    public virtual bool IsStillAlive(RapidsConnection connection) {
        return true;
    }

    public virtual void Packet(RapidsConnection connection, Packet packet, Status information) {
        AcceptedPackets.Add(packet);
        Informations.Add(information);
    }

    public void RejectedPacket(RapidsConnection connection, Packet packet, Status problems) {
        RejectedPackets.Add(packet);
        Problems.Add(problems);
    }
}

internal class TestSystemService : TestService, River.SystemListener {
    internal readonly List<Status> FormatProblems = new();
    internal readonly List<Packet> LoopPackets = new();
    
    internal TestSystemService(Rules rules) : base(rules) { }
    
    public void InvalidFormat(RapidsConnection connection, string invalidString, Status problems) {
        FormatProblems.Add(problems);
    }

    public void LoopDetected(RapidsConnection connection, Packet packet) {
        LoopPackets.Add(packet);
    }
}

internal class DeadService : TestService {
    internal DeadService(Rules rules) : base(rules) { }
    
    public override bool IsStillAlive(RapidsConnection connection) {
        return false;
    }
}

internal class LinkedService : TestService {
    private readonly string[] _forbiddenKeys;

    internal LinkedService(string[] requiredKeys, string[] forbiddenKeys) :
        base(new Rules(new RequireKeys(requiredKeys), new ForbidKeys(forbiddenKeys))) {
        _forbiddenKeys = forbiddenKeys;
    }

    public override void Packet(RapidsConnection connection, Packet packet, Status information) {
        if (_forbiddenKeys.Length != 0) {
            packet.Set(_forbiddenKeys[0], true);
            connection.Publish(packet);
        }
        base.Packet(connection, packet, information);
    }
}
