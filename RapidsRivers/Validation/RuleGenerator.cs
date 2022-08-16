/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RapidsRivers.Validation;

namespace River.Validation; 

// Factory to create Rules for a Packet
public interface RuleGenerator {
    public IList<Rule> Rules();
}