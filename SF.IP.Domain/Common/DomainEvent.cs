using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Domain.Common
{   
    public abstract class DomainEvent : INotification
    {
        protected DomainEvent()
        {
            DateOccurred = DateTime.UtcNow;
        }

        public DateTime DateOccurred { get; protected set; }
        public bool IsPublished { get; set; }
    }
}
