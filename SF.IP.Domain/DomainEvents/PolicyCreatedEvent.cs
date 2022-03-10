using SF.IP.Domain.Common;
using SF.IP.Domain.Entities;

namespace SF.IP.Domain.DomainEvents;
public class PolicyCreatedEvent : DomainEvent
{
    public PolicyCreatedEvent(InsurancePolicy policy)
    {
        this.Policy = policy;
    }

    public InsurancePolicy Policy { get; }
}
