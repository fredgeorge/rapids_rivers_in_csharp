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
        ""boolean_key"": true,
        ""date_time_key"": ""2022-03-03T00:00:00Z"",
        ""string_list_key"":[""foo"",""bar""],
        ""integer_list_key"":[2,4],
        ""detail_key"":{
            ""detail_string_key"":""upgrade"",
            ""detail_double_key"":10.75
        }
    }";
    private readonly Packet _packet = new(Original);
    private readonly TestConnection _connection = new TestConnection();

    [Fact]
    public void UnfilteredService() {
        var service = new TestService(new Rules());
        _connection.Register(service);
        _connection.Publish(_packet);
        Assert.Single(service.AcceptedPackets);
        Assert.Single(service.Informations);
        Assert.False(service.Informations[0].HasErrors());
        Assert.Empty(service.RejectedPackets);
        Assert.Empty(service.Problems);
    }

    [Fact]
    public void FilteredServices() {
        var acceptedService = new TestService(new Rules(new RequireKeys("integer_key")));
        var rejectedService = new TestService(new Rules(new ForbidKeys("integer_key")));
        _connection.Register(acceptedService);
        _connection.Register(rejectedService);
        _connection.Publish(_packet);
        Assert.Single(acceptedService.AcceptedPackets);
        Assert.False(acceptedService.Informations[0].HasErrors());
        Assert.Single(rejectedService.RejectedPackets);
        Assert.True(rejectedService.Problems[0].HasErrors());
    }

    [Fact]
    public void InvalidJson() {
        var normalService = new TestService(new Rules());
        var systemService = new TestSystemService(_connection, new Rules());
        _connection.Register(normalService);
        _connection.Register(systemService);
        _connection.Publish("{");
        Assert.Empty(normalService.Informations);
        Assert.Empty(normalService.Problems); // Not processed
        Assert.Empty(systemService.Informations);
        Assert.Empty(systemService.Problems); // Not processed here either
        Assert.Single(systemService.FormatProblems); // Handled here!
    }
}
