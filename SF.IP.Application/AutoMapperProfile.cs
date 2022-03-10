using AutoMapper;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Domain.Entities;
using SF.IP.Domain.ValueObjects;
using System;

namespace SF.IP.Application;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<InsurancePolicy, InsurancePolicyDTO>();
        CreateMap<InsurancePolicyDTO, InsurancePolicy>();

        CreateMap<Vehicle, VehicleDTO>();
        CreateMap<VehicleDTO, Vehicle>();

        CreateMap<Address, AddressDTO>();
        CreateMap<AddressDTO, Address>();

        CreateMap<PremiumPrice, PremiumPriceDTO>();
        CreateMap<PremiumPriceDTO, PremiumPrice>();

        CreateMap<string, Guid>().ConvertUsing(new GuidTypeConverter());
    }
}

public class GuidTypeConverter : ITypeConverter<string, Guid>
{
    public Guid Convert(string source, Guid destination, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return Guid.Empty;
        }
        else if (Guid.TryParse(source, out Guid conGuid))
        {
            return conGuid;
        }
        else
        {
            return Guid.Empty;
        }

    }
}
