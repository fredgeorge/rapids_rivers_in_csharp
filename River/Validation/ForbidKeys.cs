/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using River.Packets;

namespace River.Validation;

public class ForbidKeys : RuleGenerator {
    private readonly IList<string> _keys;
    public ForbidKeys(params string[] keys) {
        _keys = keys.ToList();
    }

    public IList<Rule> Rules() {
        return _keys.Select((key) => (Rule)new ForbidKey(key)).ToList();
    }

    private class ForbidKey : Rule {
        private readonly string _key;

        internal ForbidKey(string key) {
            _key = key;
        }

        public void Evaluate(Packet packet, Status status) {
            if (packet.IsMissing(_key)) status.MissingExpected(_key);
            else status.UnexpectedlyFound(_key);
        }

        public override string ToString() {
            return $"Forbid key <{_key}>";
        }
    }
}