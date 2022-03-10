
namespace SF.IP.Application.Models.InsurancePolicy.Result;
public record CreatePolicyResultDTO : BaseResultDTO
{
    public string PolicyId { get; set; } // new created policyId , here I am assuming PolicyId as PK of Policy Entity
}

