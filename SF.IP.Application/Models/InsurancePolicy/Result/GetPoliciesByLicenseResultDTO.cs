
using System.Collections.Generic;

namespace SF.IP.Application.Models.InsurancePolicy.Result;

    public record GetPoliciesByLicenseResultDTO : BaseResultDTO
    {
        public List<InsurancePolicyDTO> InsurancePolicies { get;set;}
    }

