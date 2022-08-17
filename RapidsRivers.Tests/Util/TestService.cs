/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using RapidsRivers.Packets;
using RapidsRivers.Rapids;
using RapidsRivers.Rivers;
using RapidsRivers.Validation;
using River.Validation;

namespace RapidsRivers.Tests.Util;

internal class TestService : Rivers.River.PacketListener {
    public string Name => $"TestService [{GetHashCode()}]";
    public Rules Rules { get; }
    internal readonly List<Packet> acceptedPackets = new();
    internal readonly List<Packet> rejectedPackets = new();
    internal readonly List<Status> informations = new();
    internal readonly List<Status> problems = new();

    internal TestService(RapidsConnection connection, Rules rules) {
        Rules = rules;
    }

    public bool IsStillAlive(RapidsConnection connection) {
        return true;
    }

    public void Packet(RapidsConnection connection, Packet packet, Status information) {
        acceptedPackets.Add(packet);
        informations.Add(information);
    }

    public void RejectedPacket(RapidsConnection connection, Packet packet, Status problems) {
        rejectedPackets.Add(packet);
        this.problems.Add(problems);
    }
}