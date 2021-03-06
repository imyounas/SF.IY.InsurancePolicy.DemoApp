using SF.IP.Domain.Common;
using SF.IP.Domain.ValueObjects;
using System;
using System.Collections.Generic;


namespace SF.IP.Domain.Entities;

// This will work as AggregateRoot 
public class InsurancePolicy : BaseEntity //, Some IAggregateInterface
{
    public InsurancePolicy()
    {
        //Id = Guid.NewGuid();
        Events = new List<DomainEvent>();
    }

    //public Guid Id { get; set; }
    //public List<DomainEvent> Events { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    /*
     * Sample LicenseNumbers
     * D6101-40706-60905
     * D6101 40706 60905
     * 
     */

    public string LicenseNumber { get; set; }
    public Address Address { get; set; }
    public DateTime ExpirationDate { get; set; }
    public PremiumPrice PremiumPrice { get; set; }
    public Guid VehicleDetailId { get; set; }
    public Vehicle VehicleDetail { get; set; }


    public override string ToString()
    {
        return $"{LicenseNumber}:{FirstName}-{LastName}:{EffectiveDate.ToString("dd MMMM yyyy")}";
    }
}

