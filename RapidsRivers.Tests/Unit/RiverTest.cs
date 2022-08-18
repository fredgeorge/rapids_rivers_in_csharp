/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RapidsRivers.Packets;
using RapidsRivers.Tests.Util;
using RapidsRivers.Validation;
using Xunit;
using Xunit.Abstractions;

namespace RapidsRivers.Tests.Unit; 

public class RiverTest {
    private readonly ITestOutputHelper _testOutputHelper;

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
    private readonly TestConnection _connection = new(2);
    public RiverTest(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

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
        var systemService = new TestSystemService(new Rules());
        _connection.Register(normalService);
        _connection.Register(systemService);
        _connection.Publish("{");
        Assert.Empty(normalService.Informations);
        Assert.Empty(normalService.Problems); // Not processed
        Assert.Empty(systemService.Informations);
        Assert.Empty(systemService.Problems); // Not processed here either
        Assert.Single(systemService.FormatProblems); // Handled here!
    }

    [Fact]
    public void StartUp() {
        var service = new TestService(new Rules());
        _connection.Register(service);
        Assert.Single(_connection.AllPackets);
    }

    [Fact]
    public void HeartBeats() {
        var normalService = new TestService(new Rules());
        var deadService = new DeadService(new Rules());
        var systemService = new TestSystemService(new Rules());
        _connection.Register(normalService);
        _connection.Register(deadService);
        _connection.Register(systemService);
        Assert.Empty(normalService.AcceptedPackets);
        Assert.Empty(deadService.AcceptedPackets);
        Assert.Empty(systemService.AcceptedPackets); 
        HeartBeatPacket heartBeat = new();
        _connection.Publish(heartBeat); 
        _testOutputHelper.WriteLine(string.Join("\n", _connection.AllMessages));
        Assert.Empty(normalService.AcceptedPackets);
        Assert.Equal(3, systemService.AcceptedPackets.Count); // HeartBeat + 2 responses
        _connection.Publish(heartBeat);
        _connection.Publish(heartBeat);
        // _testOutputHelper.WriteLine(string.Join("\n", _connection.AllMessages));
        Assert.Equal(9, systemService.AcceptedPackets.Count); // HeartBeat + 2 responses
    }

    [Fact]
    public void LoopDetection() {
        var systemService = new TestSystemService(new Rules());
        _connection.Register(systemService);
        var packet = Packet.Empty();
        _connection.Publish(packet);
        Assert.Single(systemService.AcceptedPackets);
        Assert.Empty(systemService.LoopPackets);
        
        packet.Set("system_read_count", 1);
        _connection.Publish(packet);
        Assert.Equal(2, systemService.AcceptedPackets.Count);
        Assert.Empty(systemService.LoopPackets);
        
        packet.Set("system_read_count", 2); // Threshold is 2; count is incremented, then checked
        _connection.Publish(packet);
        Assert.Equal(2, systemService.AcceptedPackets.Count); // Packet not sent to service
        Assert.Single(systemService.LoopPackets);
    }
}
