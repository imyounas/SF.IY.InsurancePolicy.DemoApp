
namespace SF.IP.Application.Models.InsurancePolicy;

public record VehicleDTO
{
    public string Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Name { get; set; }
}

