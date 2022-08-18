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
    internal static Rules rules = new(
        new RequireValue(PACKET_TYPE_KEY, SYSTEM_PACKET_TYPE_VALUE),
        new RequireValue(SYSTEM_PURPOSE_KEY, HEART_BEAT_PURPOSE_VALUE),
        new RequireKeys(HEART_BEAT_GENERATOR_KEY, HEART_BEAT_INDEX_KEY),
        new ForbidKeys(HEART_BEAT_RESPONDER_KEY)
    );

    private string JsonString(int index) {
        return JsonSerializer.Serialize(new Dictionary<string, object>() {
            { COMMUNITY_KEY, SYSTEM_COMMUNITY_VALUE },
            { PACKET_TYPE_KEY, SYSTEM_PACKET_TYPE_VALUE },
            { SYSTEM_PURPOSE_KEY, HEART_BEAT_PURPOSE_VALUE },
            { HEART_BEAT_GENERATOR_KEY, GetHashCode() },
            { HEART_BEAT_INDEX_KEY, index }
        });
    }

    public string ToJsonString() => JsonString(++index);
    
    public override string ToString() => JsonString(index);
}