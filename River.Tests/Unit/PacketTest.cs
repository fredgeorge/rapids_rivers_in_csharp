using System.Collections.Generic;
using River.Packets;
using Xunit;

namespace River.Tests.Unit;

public class PacketTest {
    private const string Original = @"{
        ""string_key"":""rental_offer_engine"",
        ""integer_key"":7,
        ""double_key"":7.5,
        ""null_key"":null,
        ""empty_string"":"""",
        ""boolean_key"": true,
        ""boolean_string_key"": ""false"",
        ""date_time_key"": ""2022-03-03T06:06:06Z"",
        ""string_list_key"":[""foo"",""bar""],
        ""integer_list_key"":[2,4],
        ""empty_list_key"":[]
    }";

    private readonly Packet _packet = new(Original);

    [Fact]
    public void FetchNugget() {
        Assert.Equal("rental_offer_engine", _packet.String("string_key"));
        Assert.Equal(7, _packet.Integer("integer_key"));
        Assert.Equal(7.5, _packet.Double("double_key"));
        Assert.True(_packet.Boolean("boolean_key"));
        Assert.False(_packet.Boolean("boolean_string_key"));
    }

    // [Fact]
    // public void IsMissing() {
    //     Assert.True(_packet.IsMissing("foo"));
    //     Assert.True(_packet.IsMissing("empty"));
    // }
}