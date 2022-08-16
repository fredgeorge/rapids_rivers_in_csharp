namespace River.Validation; 

// Factory to create Rules for a Packet
public interface RuleGenerator {
    public IList<Rule> Rules();
}