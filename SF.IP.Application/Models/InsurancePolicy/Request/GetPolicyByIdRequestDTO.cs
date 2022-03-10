using System;

namespace SF.IP.Application.Models.InsurancePolicy.Request
{
    public record GetPolicyByIdRequestDTO:BaseRequestDTO
    {
        public string PolicyId { get; set; } // here I am assuming PolicyId as PK of Policy Entity
        public string LicenseNumber { get; set; } // why the doc says, license number to be required here ?
    }
}
