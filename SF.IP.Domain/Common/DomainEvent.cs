using MediatR;
using System;

namespace SF.IP.Domain.Common;

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        DateOccurred = DateTime.UtcNow;
    }

    public DateTime DateOccurred { get; protected set; }
    public bool IsPublished { get; set; }
}

