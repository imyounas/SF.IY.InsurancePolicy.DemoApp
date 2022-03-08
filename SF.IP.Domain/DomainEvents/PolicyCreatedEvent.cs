using SF.IP.Domain.Common;
using SF.IP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Domain.DomainEvents
{
    public class PolicyCreatedEvent : DomainEvent
    {
        public PolicyCreatedEvent(InsurancePolicy policy)
        {
            this.Policy = policy;
        }

        public InsurancePolicy Policy { get; }
    }
}
