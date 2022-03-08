using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Models.InsurancePolicy.Request
{
    public class GetPoliciesByLicenseRequestDTO:BaseRequestDTO
    {
        public string LicenseNumber { get; set; }
        public bool SortAscByVehicleRegisterationYear { get; set; }
        public bool IncludeExpiredPolicies { get; set; }
    }
}
