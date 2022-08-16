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