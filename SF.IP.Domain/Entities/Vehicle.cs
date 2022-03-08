using SF.IP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public Vehicle()
        {
            //Id = Guid.NewGuid();
            //Events = new List<DomainEvent>();
        }

        //public List<DomainEvent> Events { get; set; }
        //public Guid Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
    }
}
