/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Text.Json;
using River.Validation;
using RapidsRivers.Packets;
using RapidsRivers.Rivers;

namespace RapidsRivers.Validation;

public class RequireValue : RuleGenerator {
    private readonly Rule _rule;
    public RequireValue(string key, string value) {
        _rule = new RequireString(key, value);
    }
    
    public RequireValue(string key, double value) {
        _rule = new RequireNumber(key, value);
    }
    
    public RequireValue(string key, bool value) {
        _rule = new RequireBoolean(key, value);
    }

    public IList<Rule> Rules() {
        return new List<Rule>() { _rule };
    }
    
    private class RequireString : Rule {
        private readonly string _key;
        private readonly string _requiredValue;

        internal RequireString(string key, string requiredValue) {
            _key = key;
            _requiredValue = requiredValue;
        }

        public void Evaluate(Packet packet, Status status) {
            if (packet.IsMissing(_key)) status.UnexpectedlyMissing(_key);
            else if (packet.Has(_key, JsonValueKind.String) && packet.String(_key) == _requiredValue)
                status.FoundValue(_key, _requiredValue);
            else status.MissingValue(_key, _requiredValue);
        }

        public override string ToString() {
            return $"Require key <{_key}> has value <{_requiredValue}>";
        }
    }
    
    private class RequireNumber : Rule {
        private readonly string _key;
        private readonly double _requiredValue;

        internal RequireNumber(string key, double requiredValue) {
            _key = key;
            _requiredValue = requiredValue;
        }

        public void Evaluate(Packet packet, Status status) {
            if (packet.IsMissing(_key)) status.UnexpectedlyMissing(_key);
            else if (packet.Has(_key, JsonValueKind.Number) && packet.Double(_key) == _requiredValue)
                status.FoundValue(_key, _requiredValue);
            else status.MissingValue(_key, _requiredValue);
        }

        public override string ToString() {
            return $"Require key <{_key}> has value <{_requiredValue}>";
        }
    }
    
    private class RequireBoolean : Rule {
        private readonly string _key;
        private readonly bool _requiredValue;

        internal RequireBoolean(string key, bool requiredValue) {
            _key = key;
            _requiredValue = requiredValue;
        }

        public void Evaluate(Packet packet, Status status) {
            if (packet.IsMissing(_key)) status.UnexpectedlyMissing(_key);
            else if ((packet.Has(_key, JsonValueKind.True) || packet.Has(_key, JsonValueKind.False)) 
                     && packet.Boolean(_key) == _requiredValue)
                status.FoundValue(_key, _requiredValue);
            else status.MissingValue(_key, _requiredValue);
        }

        public override string ToString() {
            return $"Require key <{_key}> has value <{_requiredValue}>";
        }
    }
}