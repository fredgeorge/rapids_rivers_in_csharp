using System.Security;
using River.Packets;
using River.Validation;
using Xunit;

namespace River.Tests.Unit;

public class ValidationTest {
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

    [Fact]
    public void RequiredKeys() {
        Assert.True(_packet.DoesPass(new Rules(
            new RequireKeys("string_key", "integer_key")
        )));
        Assert.False(_packet.DoesPass(new Rules(
            new RequireKeys("string_key", "foo")
        )));
    }
}