using System.Text.Json;

namespace River.Packets; 

// Understands a specific message on an Event Bus
public class Packet {
    private readonly Dictionary<string, JsonElement> _map;

    public Packet(string jsonString) {
        if (string.IsNullOrEmpty(jsonString))
            throw new ArgumentException("JSON string cannot be null or empty", nameof(jsonString));
        try {
            _map = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString)
                                ?? throw new ArgumentException("JSON string could not be deserialized", nameof(jsonString));
        } catch (Exception ex) {
            throw new ArgumentException("JSON string could not be deserialized", nameof(jsonString), ex);
        }
    }

    public bool Has(string key) =>
        _map.ContainsKey(key)
        && (_map[key].ValueKind switch {
            JsonValueKind.Null => false,
            JsonValueKind.String when _map[key].GetString() == string.Empty => false,
            _ => true
        });

    public bool IsMissing(string key) {
        return !Has(key);
    }

    public string String(string key) {
        return Element(key, JsonValueKind.String).GetString();
    }

    public int Integer(string key) {
        return Element(key, JsonValueKind.Number).GetInt32();
    }

    public double Double(string key) {
        return Element(key, JsonValueKind.Number).GetDouble();
    }

    public bool Boolean(string key) {
        if (Has(key, JsonValueKind.True)) return true;
        if (Has(key, JsonValueKind.False)) return false;
        if (IsBool(key, out var parsedValue)) return parsedValue;
        throw new ArgumentException($"Value of <{key}> is of type {_map[key].ValueKind} rather than expected type of Boolean");
    }

    private JsonElement Element(string key, JsonValueKind kind) {
        if (IsMissing(key)) throw new ArgumentException($"Key <{key}> does not exist", nameof(key));
        if (_map[key].ValueKind != kind)
            throw new ArgumentException($"Value of <{key}> is of type {_map[key].ValueKind} rather than expected type of {kind}", nameof(key));
        return _map[key];
    }

    private bool Has(string key, JsonValueKind kind) {
        return Has(key) && _map[key].ValueKind == kind;
    }
    
    private bool IsBool(string key, out bool parsedValue) {
        parsedValue = false;
        return Has(key, JsonValueKind.String) &&
               bool.TryParse(_map[key].GetString(), out parsedValue);
    }
}