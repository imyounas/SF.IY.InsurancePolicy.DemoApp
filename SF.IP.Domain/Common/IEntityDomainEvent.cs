
using System.Collections.Generic;


namespace SF.IP.Domain.Common;

public interface IEntityDomainEvent
{
    public List<DomainEvent> Events { get; set; }
}

