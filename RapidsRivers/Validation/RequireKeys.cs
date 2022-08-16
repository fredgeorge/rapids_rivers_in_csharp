/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using River.Validation;
using RapidsRivers.Packets;
using RapidsRivers.Rivers;

namespace RapidsRivers.Validation;

public class RequireKeys : RuleGenerator {
    private readonly IList<string> _keys;
    public RequireKeys(params string[] keys) {
        _keys = keys.ToList();
    }

    public IList<Rule> Rules() {
        return _keys.Select((key) => (Rule)new RequireKey(key)).ToList();
    }
    
    private class RequireKey : Rule {
        private readonly string _key;

        internal RequireKey(string key) {
            _key = key;
        }

        public void Evaluate(Packet packet, Status status) {
            if (packet.IsMissing(_key)) status.UnexpectedlyMissing(_key);
            else status.FoundExpected(_key);
        }

        public override string ToString() {
            return $"Require key <{_key}>";
        }
    }
}