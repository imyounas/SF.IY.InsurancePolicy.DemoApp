namespace SF.IP.Application.Models.InsurancePolicy.Request;

public class GetPoliciesByLicenseRequestDTO : BaseRequestDTO
{
    public string LicenseNumber { get; set; }
    public bool SortAscByVehicleRegisterationYear { get; set; } //Optional
    public bool IncludeExpiredPolicies { get; set; } //Optional
}

