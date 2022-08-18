/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Collections.Generic;
using RapidsRivers.Packets;
using RapidsRivers.Rapids;
using RapidsRivers.Rivers;

namespace RapidsRivers.Tests.Util; 

internal class TestConnection : RapidsConnection {
    private readonly int _maxReadCount;
    private readonly List<River> _rivers = new();
    private readonly Queue<string> _messages = new();
    internal readonly List<RapidsPacket> AllPackets = new();
    internal readonly List<string> AllMessages = new();

    internal TestConnection(int maxReadCount = 9) {
        _maxReadCount = maxReadCount;
    }
    
    public void Register(River.PacketListener listener) {
        var river = new River(this, listener.Rules, _maxReadCount);
        river.Register(listener);
        _rivers.Add(river);
    }

    public void Register(River.SystemListener listener)  {
        var river = new River(this, listener.Rules, _maxReadCount);
        river.Register(listener);
        _rivers.Add(river);
    }

    internal void Publish(string message) {
        AllMessages.Add(message);
        if (_messages.Count > 0) _messages.Enqueue(message);
        else {
            _messages.Enqueue(message);
            while (_messages.Count > 0) {
                var nextMessage = _messages.Peek();
                _rivers.ForEach(river => river.Message(this, nextMessage));
                _messages.Dequeue();
            }
        }
    }

    public void Publish(RapidsPacket packet) {
        AllPackets.Add(packet);
        Publish(packet.ToJsonString());
    }
}
