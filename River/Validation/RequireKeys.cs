/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using River.Packets;

namespace River.Validation;

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

        public bool IsValid(Packet packet) {
            return packet.Has(_key);
        }
    }
}