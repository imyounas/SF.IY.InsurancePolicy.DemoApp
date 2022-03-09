using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Domain.Common
{
    public abstract class BaseEntity
    {
        
        public Guid Id { get; set; }

        // in real application we could have separate interface, or base class for domain events.
        // so only entities or AggregateRoots could implement that interface
        public List<DomainEvent> Events { get; set; }

        // similarly we could have separate interface for this auditing behaviour, so only entities which need could implement it
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
