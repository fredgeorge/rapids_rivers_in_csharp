using System.Collections.Generic;
using River.Packets;
using Xunit;

namespace River.Tests.Unit;

public class PacketTest {
    private readonly Packet _packet = new Packet(new Dictionary<string, object>() {
        { "category", "car_offer_engine" },
        { "need", "car_rental_offer" },
        { "read_count", 3 }
    });

    [Fact]
    public void FetchNugget() {
        Assert.Equal("car_offer_engine", _packet["category"]);
        Assert.Equal(3, _packet["read_count"]);
    }
}