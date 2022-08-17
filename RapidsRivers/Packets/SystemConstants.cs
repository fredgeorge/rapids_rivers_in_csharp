/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace RapidsRivers.Packets; 

internal static class SystemConstants {
    internal const string COMMUNITY_KEY = "community";
    internal const string SYSTEM_COMMUNITY_VALUE = "system";
    
    internal const string PACKET_TYPE_KEY = "packet_type";
    internal const string SYSTEM_PACKET_TYPE_VALUE = "system_packet";
    
    internal const string SYSTEM_PURPOSE_KEY = "system_purpose";
    internal const string START_UP_SYSTEM_PURPOSE_VALUE = "start_up";
    internal const string HEART_BEAT_PURPOSE_VALUE = "heart_beat";

    internal const string SERVICE_NAME_KEY = "service_name";
    
    internal const string HEART_BEAT_GENERATOR_KEY = "heart_beat_generator";
    
    internal const string HEART_BEAT_RESPONDER_KEY = "heart_beat_responder";
    
    internal const string HEART_BEAT_INDEX_KEY = "heart_beat_index";
}
