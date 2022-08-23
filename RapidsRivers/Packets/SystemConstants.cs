/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace RapidsRivers.Packets; 

internal static class SystemConstants {
    internal const string CommunityKey = "community";
    internal const string SystemCommunityValue = "system";
    
    internal const string SystemReadCountKey = "system_read_count";
    
    internal const string SystemBreadCrumbsKey = "breadcrumbs";
    
    internal const string PacketTypeKey = "packet_type";
    internal const string SystemPacketTypeValue = "system_packet";
    
    internal const string SystemPurposeKey = "system_purpose";
    internal const string StartUpSystemPurposeValue = "start_up";
    internal const string HeartBeatPurposeValue = "heart_beat";

    internal const string ServiceNameKey = "service_name";
    
    internal const string HeartBeatGeneratorKey = "heart_beat_generator";
    
    internal const string HeartBeatResponderKey = "heart_beat_responder";
    
    internal const string HeartBeatIndexKey = "heart_beat_index";
}
