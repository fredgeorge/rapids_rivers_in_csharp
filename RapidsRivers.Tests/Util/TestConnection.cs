using System;
using System.Collections.Generic;
using RapidsRivers.Packets;
using RapidsRivers.Rapids;

namespace River.Tests.Util; 

public class TestConnection: RapidsConnection {
    private readonly List<RapidsConnection.MessageListener> _rivers = new();
    private readonly List<string> _sentMessages = new();
    
    public void Register(RapidsRivers.Rivers.River.PacketListener listener) {
        var river = new RapidsRivers.Rivers.River(this, listener.rules, 0);
        _rivers.Add(river);
        river.register(listener);
    }

    public void Register(RapidsRivers.Rivers.River.SystemListener listener) {
        throw new NotImplementedException();
    }

    public void Publish(RapidsPacket packet) {
        _sentMessages.Add(packet.ToJsonString());
    }

    internal void InjectMessage(string content) {
        _rivers.ForEach((river) => river.Message(this, content));
    }
}