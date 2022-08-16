using System.Text.Json;
using River.Packets;

namespace River.Validation;

public class RequireValue : RuleGenerator {
    private readonly Rule _rule;
    public RequireValue(string key, string value) {
        _rule = new RequireString(key, value);
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

        public bool IsValid(Packet packet) {
            return packet.Has(_key, JsonValueKind.String) && packet.String(_key) == _requiredValue;
        }
    }
}