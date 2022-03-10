
namespace SF.IP.Application.Models.InsurancePolicy.Request;

public record CreatePolicyRequestDTO : BaseRequestDTO
{
    public InsurancePolicyDTO InsurancePolicy { get; set; }
}

