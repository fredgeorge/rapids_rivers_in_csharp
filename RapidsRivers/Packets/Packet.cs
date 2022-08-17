/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Text.Json;
using RapidsRivers.Rivers;
using River.Validation;
using RapidsRivers.Validation;

namespace RapidsRivers.Packets; 

// Understands a specific message on an Event Bus
public class Packet : RapidsPacket {
    private readonly Dictionary<string, JsonElement> _map;

    public Packet(string jsonString) {
        if (string.IsNullOrEmpty(jsonString))
            throw new PacketException("JSON string cannot be null or empty", nameof(jsonString));
        try {
            _map = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString)
                                ?? throw new PacketException("JSON string could not be deserialized", nameof(jsonString));
        } catch (Exception ex) {
            throw new PacketException("JSON string could not be deserialized", nameof(jsonString), ex);
        }
    }

    private bool Has(string key) =>
        _map.ContainsKey(key)
        && (_map[key].ValueKind switch {
            JsonValueKind.Null => false,
            JsonValueKind.String when _map[key].GetString() == string.Empty => false,
            JsonValueKind.Array when _map[key].GetArrayLength() == 0 => false,
            _ => true
        });

    public bool IsMissing(string key) {
        return !Has(key);
    }

    public string String(string key) {
        return Element(key, JsonValueKind.String).GetString()!;
    }

    public int Integer(string key) {
        var value = Double(key);
        if (value % 1 != 0) throw new PacketException($"Value of <{key}> is a double, not an integer");
        return (int)value;
    }

    public double Double(string key) {
        return Element(key, JsonValueKind.Number).GetDouble();
    }

    public DateTime DateTime(string key) {
        if (IsDateTime(key, out var parsedValue)) return parsedValue;
        throw new PacketException("Value of <{key}> is of type {_map[key].ValueKind} rather than expected type of DateTime", nameof(key));
    }

    public bool Boolean(string key) {
        if (Has(key, JsonValueKind.True)) return true;
        if (Has(key, JsonValueKind.False)) return false;
        if (IsBool(key, out var parsedValue)) return parsedValue;
        throw new PacketException($"Value of <{key}> is of type {_map[key].ValueKind} rather than expected type of Boolean");
    }

    public Packet this[string key] => new(Element(key, JsonValueKind.Object).GetRawText());

    public Status Evaluate(Rules rules) {
        var result = new Status();
        foreach (var rule in rules) {
            rule.Evaluate(this, result);
        }
        return result;
    }

    public string ToJsonString() => JsonSerializer.Serialize(_map);

    private JsonElement Element(string key, JsonValueKind kind) {
        if (IsMissing(key)) throw new PacketException($"Key <{key}> does not exist", nameof(key));
        if (_map[key].ValueKind != kind)
            throw new PacketException($"Value of <{key}> is of type {_map[key].ValueKind} rather than expected type of {kind}", nameof(key));
        return _map[key];
    }

    internal bool Has(string key, JsonValueKind kind) {
        return Has(key) && _map[key].ValueKind == kind;
    }
    
    private bool IsBool(string key, out bool parsedValue) {
        parsedValue = false;
        return Has(key, JsonValueKind.String) &&
               bool.TryParse(_map[key].GetString(), out parsedValue);
    }

    private bool IsDateTime(string key, out DateTime parsedValue) {
        parsedValue = default;
        return Has(key, JsonValueKind.String) && this._map[key].TryGetDateTime(out parsedValue);
    }
}

public class PacketException : ArgumentException {
    public PacketException(string message, string? paramName = null) 
        : base(message, paramName) { }
    public PacketException(string? message, string? paramName, Exception? innerException) 
        : base(message, paramName, innerException) { }
    
}