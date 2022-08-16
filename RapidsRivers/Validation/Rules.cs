/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Collections;
using River.Validation;

namespace RapidsRivers.Validation; 

// Understands filtering criteria for Packet
public class Rules : IEnumerable<Rule> {
    private readonly List<Rule> _rules = new();
    public Rules(params RuleGenerator[] generators) {
        foreach (var generator in generators) {
            _rules.AddRange(generator.Rules());
        }
    }

    public IEnumerator<Rule> GetEnumerator() {
        foreach(var rule in _rules)
        {
            yield return rule;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}