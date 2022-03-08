using SF.IP.Application.Interfaces.StateRegulation;
using SF.IP.Application.Models.InsurancePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Infrastructure.StateRegulation
{
    public class PolicyStateRegulator : IPolicyStateRegulator
    {
        public Task<(bool status, string reason)> ValidatePolicyFromStateRegulator(InsurancePolicyDTO policy)
        {
            throw new NotImplementedException();
        }
    }
}
