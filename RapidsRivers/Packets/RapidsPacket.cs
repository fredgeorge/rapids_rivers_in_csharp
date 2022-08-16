/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace RapidsRivers.Packets; 

public interface RapidsPacket {
    string ToJsonString();
}