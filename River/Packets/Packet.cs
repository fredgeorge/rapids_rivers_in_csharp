namespace River.Packets; 

// Understands a specific message on an Event Bus
public class Packet {
    private readonly Dictionary<string, object> _map;

    public Packet(Dictionary<string, object> map) {
        _map = map;
    }

    public object this[string key] {
        get { return _map[key]; }
    }
}