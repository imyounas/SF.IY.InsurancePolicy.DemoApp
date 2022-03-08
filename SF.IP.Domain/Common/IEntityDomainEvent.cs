using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Domain.Common
{
    public interface IEntityDomainEvent
    {
        public List<DomainEvent> Events { get; set; }
    }
}
