using SF.IP.Application.Models.InsurancePolicy;
using System.Threading.Tasks;

namespace SF.IP.Application.Interfaces.StateRegulation;

public interface IPolicyStateRegulator
{
    Task<(bool Status, string Reason)> ValidatePolicyFromStateRegulator(InsurancePolicyDTO policy);
}

