/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using River.Packets;

namespace River.Validation; 

// Understands a particular criteria a Packet must meet
public interface Rule {
    void Evaluate(Packet packet, Status status);
}