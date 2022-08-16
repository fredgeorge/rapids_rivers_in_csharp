/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace River.Packets;

public class Status {
    private readonly List<string> _informationalMessages = new();
    private readonly List<string> _errorMessages = new();
    
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
        _informationalMessages.Add($"Require key <{key}> has value <{requiredValue}>");
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
}