/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Text.Json;
using RapidsRivers.Rivers;
using static RapidsRivers.Packets.SystemConstants;

namespace RapidsRivers.Packets;

public class StartUpPacket : RapidsPacket {
    private readonly string _serviceName;

    public StartUpPacket(River.PacketListener listener) {
        _serviceName = listener.Name;
    }

    public string ToJsonString() {
        return JsonSerializer.Serialize(new Dictionary<string, object>() {
            { CommunityKey, SystemCommunityValue },
            { PacketTypeKey, SystemPacketTypeValue },
            { SystemPurposeKey, StartUpSystemPurposeValue },
            { ServiceNameKey, _serviceName }
        });
    }
}