/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Text;

namespace RapidsRivers.Rivers;

public class Status {
    private readonly string _originalPacketString;
    private readonly List<string> _informationalMessages = new();
    private readonly List<string> _errorMessages = new();

    internal Status(string originalPacketString) {
        _originalPacketString = originalPacketString;
    }
    
    public bool HasErrors() {
        return _errorMessages.Count > 0;
    }

    public void FoundExpected(string key) {
        _informationalMessages.Add($"Required key of <{key}> was found");
    }

    public void MissingExpected(string key) {
        _informationalMessages.Add($"Forbidden key of <{key}> was not found");
    }

    public void FoundValue(string key, object requiredValue) {
        _informationalMessages.Add($"Required key <{key}> has value <{requiredValue}>");
    }

    public void UnexpectedlyMissing(string key) {
        _errorMessages.Add($"Required key of <{key}> is missing");
    }

    public void UnexpectedlyFound(string key) {
        _errorMessages.Add($"Forbidden key of <{key}> unexpectedly exists");
    }

    public void MissingValue(string key, object requiredValue) {
        _errorMessages.Add($"Required key of <{key}> is missing required value of <{requiredValue}>");
    }

    public void Error(string message) {
        _errorMessages.Add(message);
    }

    public override string ToString() {
        var result = new StringBuilder("Status of filtering of:\n");
        result.Append($"\tOriginal packet: {_originalPacketString}\n");
        AppendTo(result, "Errors", _errorMessages);
        AppendTo(result, "Informational messages", _informationalMessages);
        return result.ToString();
    }

    private void AppendTo(StringBuilder builder, string category, List<string> messages) {
        if (messages.Count == 0) return;
        builder.Append($"\t{category} - {messages.Count}:");
        messages.ForEach((message) => {
            builder.Append("\t\t");
            builder.Append(message);
            builder.Append('\n');
        });
    }
}