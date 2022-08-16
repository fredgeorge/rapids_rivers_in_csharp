using River.Packets;

namespace River.Validation; 

// Understands a particular criteria a Packet must meet
public interface Rule {
    bool IsValid(Packet packet);
}