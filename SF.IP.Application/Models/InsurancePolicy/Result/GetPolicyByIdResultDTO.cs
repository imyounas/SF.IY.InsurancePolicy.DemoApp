
namespace SF.IP.Application.Models.InsurancePolicy.Result;

public record GetPolicyByIdResultDTO : BaseResultDTO
{
    public InsurancePolicyDTO InsurancePolicy { get; set; }
}

