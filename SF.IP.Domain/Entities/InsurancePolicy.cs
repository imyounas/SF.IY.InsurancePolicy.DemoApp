using SF.IP.Domain.Common;
using SF.IP.Domain.DomainEvents;
using SF.IP.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Domain.Entities
{
    /*
     * 
     * 1.	Required Params: Effective Date, First Name, Last Name, Drivers License #, 
     * Vehicle details (Year, Model, Manufacturer, Vehicle Name), Address, Expiration Date, Premium (price). 
     * */
    // This will work as AggregateRoot 
    public class InsurancePolicy : BaseEntity
    {
        public InsurancePolicy()
        {
            //Id = Guid.NewGuid();
            //Events = new List<DomainEvent>();
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

        //public void AddNewPolicyCreatedEvent()
        //{
        //    Events.Add(new PolicyCreatedEvent(this));
        //}
    }
}
