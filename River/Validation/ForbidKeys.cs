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

        public bool IsValid(Packet packet) {
            return packet.IsMissing(_key);
        }
    }
}