/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RapidsRivers.Packets;
using static RapidsRivers.Rivers.River;

namespace RapidsRivers.Rapids; 

// Understands access to an undifferentiated stream of messages
public interface RapidsConnection {
    void Register(PacketListener listener);
    void Register(SystemListener listener);
    void Publish(RapidsPacket packet);
    
    public interface MessageListener {
        void Message(RapidsConnection connection, string message);
    }
}