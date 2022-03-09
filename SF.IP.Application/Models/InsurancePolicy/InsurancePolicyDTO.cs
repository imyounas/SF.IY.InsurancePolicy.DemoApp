using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Models.InsurancePolicy
{
    public record InsurancePolicyDTO
    {
        public string Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public AddressDTO Address { get; set; }
        public PremiumPriceDTO PremiumPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string VehicleDetailId { get; set; }
        public VehicleDTO VehicleDetail { get; set; }

        public override string ToString()
        {
            return $"{LicenseNumber}:{FirstName}-{LastName}:{EffectiveDate.ToString("dd MMMM yyyy")}";
        }

    }

    public record AddressDTO
    {
        //[Required(ErrorMessage = "Street in Address is required")]
        public string Street { get;  set; }
        //[Required(ErrorMessage = "City in Address is required")]
        public string City { get;  set; }
        //[Required(ErrorMessage = "State in Address is required")]
        public string State { get;  set; }
        
        //[Required(ErrorMessage = "Country in Address is required")]
        //public string Country { get; private set; }
        
        //[Required(ErrorMessage = "ZipCode in Address is required")]
        public string ZipCode { get;  set; }

    }

    public record PremiumPriceDTO
    {
        //[Required(ErrorMessage = "Price is required")]
        public double Price { get; private set; }
        //[Required(ErrorMessage = "Currency is required")]
        public string Currency { get; private set; }
    }
}
