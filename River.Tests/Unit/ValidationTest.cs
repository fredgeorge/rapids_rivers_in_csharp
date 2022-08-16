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
    public void NoRules() {
        Assert.True(_packet.DoesPass(new Rules()));
    }

    [Fact]
    public void RequiredKeys() {
        Assert.True(_packet.DoesPass(new Rules(
            new RequireKeys("string_key", "integer_key")
        )));
        Assert.False(_packet.DoesPass(new Rules(
            new RequireKeys("string_key", "foo")
        )));
    }

    [Fact]
    public void ForbiddenKeys() {
        Assert.True(_packet.DoesPass(new Rules(new ForbidKeys("foo"))));
        Assert.False(_packet.DoesPass(new Rules(new ForbidKeys("string_key", "foo"))));
        Assert.True(_packet.DoesPass(new Rules(new ForbidKeys("null_key", "empty_string", "empty_list_key"))));
    }

    [Fact]
    public void RequireSpecificString() {
        Assert.True(_packet.DoesPass(new Rules(new RequireValue("string_key", "rental_offer_engine"))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("string_key", "foo"))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("bar", "foo"))));
    }

    [Fact]
    public void RequireSpecificNumber() {
        Assert.True(_packet.DoesPass(new Rules(new RequireValue("integer_key", 7))));
        Assert.True(_packet.DoesPass(new Rules(new RequireValue("integer_key", 7.0))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("integer_key", 8))));
        Assert.True(_packet.DoesPass(new Rules(new RequireValue("double_key", 7.5))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("double_key", 8))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("integer_key", "foo"))));
    }

    [Fact]
    public void RequireSpecificBoolean() {
        Assert.True(_packet.DoesPass(new Rules(new RequireValue("boolean_key", true))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("boolean_key", false))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("boolean_string_key", "true"))));
        Assert.True(_packet.DoesPass(new Rules(new RequireValue("boolean_string_key", "false"))));
        Assert.False(_packet.DoesPass(new Rules(new RequireValue("boolean_key", "foo"))));
    }
}