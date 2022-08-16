using River.Packets;

namespace River.Validation; 

// Factory to create Rules for a Packet
public interface RuleGenerator {
    public IList<Rule> Rules();
}

public class RequireKeys : RuleGenerator {
    private readonly IList<string> _keys;
    public RequireKeys(params string[] keys) {
        _keys = keys.ToList();
    }

    public IList<Rule> Rules() {
        return _keys.Select((key) => (Rule)new RequireKey(key)).ToList();
    }
}

internal class RequireKey : Rule {
    private readonly string _key;

    internal RequireKey(string key) {
        _key = key;
    }

    public bool IsValid(Packet packet) {
        return packet.Has(_key);
    }
}

public class ForbidKeys : RuleGenerator {
    private readonly IList<string> _keys;
    public ForbidKeys(params string[] keys) {
        _keys = keys.ToList();
    }

    public IList<Rule> Rules() {
        return _keys.Select((key) => (Rule)new ForbidKey(key)).ToList();
    }
}

internal class ForbidKey : Rule {
    private readonly string _key;

    internal ForbidKey(string key) {
        _key = key;
    }

    public bool IsValid(Packet packet) {
        return packet.IsMissing(_key);
    }
}