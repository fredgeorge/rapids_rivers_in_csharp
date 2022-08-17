/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RapidsRivers.Packets;
using RapidsRivers.Tests.Util;
using RapidsRivers.Validation;
using Xunit;

namespace RapidsRivers.Tests.Unit; 

public class RiverTest {
    private const string Original = @"{
        ""string_key"":""rental_offer_engine"",
        ""integer_key"":7,
        ""double_key"":7.5,
        ""null_key"":null,
        ""empty_string"":"""",
        ""boolean_key"": true,
        ""boolean_string_key"": ""false"",
        ""date_time_key"": ""2022-03-03T00:00:00Z"",
        ""string_list_key"":[""foo"",""bar""],
        ""integer_list_key"":[2,4],
        ""empty_list_key"":[],
        ""detail_key"":{
            ""detail_string_key"":""upgrade"",
            ""detail_double_key"":10.75
        }
    }";
    private readonly Packet _packet = new(Original);
    private readonly TestConnection _connection = new TestConnection();

    [Fact]
    public void UnfilteredService() {
        var service = new TestService(_connection, new Rules());
        _connection.Register(service);
        _connection.Publish(_packet);
        Assert.Single(service.acceptedPackets);
        Assert.Single(service.informations);
        Assert.False(service.informations[0].HasErrors());
        Assert.Empty(service.rejectedPackets);
        Assert.Empty(service.problems);
    }

}