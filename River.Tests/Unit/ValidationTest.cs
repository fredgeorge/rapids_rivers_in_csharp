/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using River.Packets;
using River.Validation;
using Xunit;

namespace River.Tests.Unit;

// Ensures Rules execute properly
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
        AssertPasses(new Rules());
    }

    [Fact]
    public void RequiredKeys() {
        AssertPasses(new Rules( new RequireKeys("string_key", "integer_key") ));
        AssertFails(new Rules( new RequireKeys("string_key", "foo") ));
        AssertPasses(new Rules( new RequireKeys("detail_key") ));
    }

    [Fact]
    public void ForbiddenKeys() {
        AssertPasses(new Rules(new ForbidKeys("foo")));
        AssertFails(new Rules(new ForbidKeys("string_key", "foo")));
        AssertPasses(new Rules(new ForbidKeys("null_key", "empty_string", "empty_list_key")));
    }

    [Fact]
    public void RequireSpecificString() {
        AssertPasses(new Rules(new RequireValue("string_key", "rental_offer_engine")));
        AssertFails(new Rules(new RequireValue("string_key", "foo")));
        AssertFails(new Rules(new RequireValue("bar", "foo")));
    }

    [Fact]
    public void RequireSpecificNumber() {
        AssertPasses(new Rules(new RequireValue("integer_key", 7)));
        AssertPasses(new Rules(new RequireValue("integer_key", 7.0)));
        AssertFails(new Rules(new RequireValue("integer_key", 8)));
        AssertPasses(new Rules(new RequireValue("double_key", 7.5)));
        AssertFails(new Rules(new RequireValue("double_key", 8)));
        AssertFails(new Rules(new RequireValue("integer_key", "foo")));
    }

    [Fact]
    public void RequireSpecificBoolean() {
        AssertPasses(new Rules(new RequireValue("boolean_key", true)));
        AssertFails(new Rules(new RequireValue("boolean_key", false)));
        AssertFails(new Rules(new RequireValue("boolean_string_key", "true")));
        AssertPasses(new Rules(new RequireValue("boolean_string_key", "false")));
        AssertFails(new Rules(new RequireValue("boolean_key", "foo")));
    }

    [Fact]
    public void CompoundRules() {
        AssertPasses(new Rules(
            new RequireKeys("string_key", "integer_key"),
            new RequireValue("boolean_key", true),
            new ForbidKeys("null_key", "empty_string", "empty_list_key"),
            new RequireKeys("detail_key", "boolean_string_key")
        ));
    }

    private void AssertPasses(Rules rules) {
        Assert.False(_packet.Evaluate(rules).HasErrors());
    }

    private void AssertFails(Rules rules) {
        Assert.True(_packet.Evaluate(rules).HasErrors());
    }
}