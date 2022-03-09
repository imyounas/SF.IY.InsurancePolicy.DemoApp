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
        public async Task<(bool Status, string Reason)> ValidatePolicyFromStateRegulator(InsurancePolicyDTO policy)
        {
            var random = new Random().Next(1, 5);
            (bool Status, string Reason) validationResult = (false, "Not in mood to approve this policy, cause I can !");
            if (random % 2 == 0)
            {
                validationResult = (true, "Okay, I approve it, cause I can !");
            }

            // real application will have here 3rd party API call for which we need to wait.
            return await Task.FromResult(validationResult);

        }
    }
}
