using SF.IP.Application.Models.InsurancePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Interfaces.StateRegulation
{
    public interface IPolicyStateRegulator
    {
        Task<(bool Status, string Reason)> ValidatePolicyFromStateRegulator(InsurancePolicyDTO policy);
    }
}
