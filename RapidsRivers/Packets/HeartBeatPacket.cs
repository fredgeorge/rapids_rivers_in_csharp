/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Text.Json;
using RapidsRivers.Validation;
using static RapidsRivers.Packets.SystemConstants;

namespace RapidsRivers.Packets;

public class HeartBeatPacket : RapidsPacket {
    private int index = 0;

    // Filter for River handling heart beats
    internal static readonly Rules Rules = new(
        new RequireValue(PacketTypeKey, SystemPacketTypeValue),
        new RequireValue(SystemPurposeKey, HeartBeatPurposeValue),
        new RequireKeys(HeartBeatGeneratorKey, HeartBeatIndexKey),
        new ForbidKeys(HeartBeatResponderKey)
    );

    private string JsonString(int index) {
        return JsonSerializer.Serialize(new Dictionary<string, object>() {
            { CommunityKey, SystemCommunityValue },
            { PacketTypeKey, SystemPacketTypeValue },
            { SystemPurposeKey, HeartBeatPurposeValue },
            { HeartBeatGeneratorKey, GetHashCode() },
            { HeartBeatIndexKey, index }
        });
    }

    public string ToJsonString() => JsonString(++index);
    
    public override string ToString() => JsonString(index);
}