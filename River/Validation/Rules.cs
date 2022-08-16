using System.Collections;

namespace River.Validation; 

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