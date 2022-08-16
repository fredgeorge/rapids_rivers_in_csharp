/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RapidsRivers.Packets;

namespace RapidsRivers.Rapids; 

// Understands access to an undifferentiated stream of messages
public interface RapidsConnection {
    void Register(Rivers.River.PacketListener listener);
    void Register(Rivers.River.SystemListener listener);
    void Publish(RapidsPacket packet);
    
    public interface MessageListener {
        void Message(RapidsConnection connection, string message);
    }
}