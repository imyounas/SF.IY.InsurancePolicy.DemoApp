using SF.IP.Domain.Common;
using System.Collections.Generic;

namespace SF.IP.Domain.Entities;

public class Vehicle : BaseEntity
{
    public Vehicle()
    {
        //Id = Guid.NewGuid();
        Events = new List<DomainEvent>();
    }

    public int Year { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Name { get; set; }
}

