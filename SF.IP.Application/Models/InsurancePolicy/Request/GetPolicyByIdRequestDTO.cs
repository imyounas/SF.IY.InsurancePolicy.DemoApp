using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Models.InsurancePolicy.Request
{
    public class GetPolicyByIdRequestDTO:BaseRequestDTO
    {
        public Guid PolicyId { get; set; }
    }
}
