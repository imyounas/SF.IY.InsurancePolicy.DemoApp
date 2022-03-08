using AutoMapper;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Domain.Entities;
using SF.IP.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application
{
    public class AutoMapperProfile: Profile
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
        }
    }
}
