using FluentValidation.TestHelper;
using SF.IP.Application.Common;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Application.Validators.PolicyInsurance;
using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace SF.IP.Tests;

public class InsurancePolicyUnitTest : BaseServiceUnitTest
{
    private readonly PolicyValidator _policyValidator;
    public InsurancePolicyUnitTest()
    {
        _policyValidator = new PolicyValidator(MockContext);
        SetUpPolicyUSZipCodes();
    }

    [Fact]
    [Description("This Unit test, is validating that when invalid LastName is provided then validator service is returning correct error")]
    public void LastNameIsInvalid()
    {
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Imran",
            LastName = "",
            EffectiveDate = DateTime.Now.AddDays(31),
            LicenseNumber = "D6101-40706-60905",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { City = "las vegas", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1996 }

        };

        var result = _policyValidator.TestValidate(policyDTO);
        Assert.False(result.IsValid);
        Assert.Contains(SFConstants.INVALID_LASTNAME, result.Errors.Select(x => x.ErrorCode));
    }

    [Fact]
    [Description("This Unit test, is validating that when invalid LicenseNumber is provided then validator service is returning correct error")]
    public void LicenseNumberShouldBeOfInCorrectFormat()
    {
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Imran",
            LastName = "Khan",
            EffectiveDate = DateTime.Now.AddDays(31),
            LicenseNumber = "D6101-40706",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { City = "las vegas", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1996 }

        };

        var result = _policyValidator.TestValidate(policyDTO);
        Assert.False(result.IsValid);
        Assert.Contains(SFConstants.INVALID_LICENSE_NUMBER, result.Errors.Select(x => x.ErrorCode));
    }

    [Fact]
    [Description("This Unit test, is validating that when valid LicenseNumber is provided then validator service is accepting it")]
    public void LicenseNumberShouldBeOfCorrectFormat()
    {
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Imran",
            LastName = "Khan",
            EffectiveDate = DateTime.Now.AddDays(31),
            LicenseNumber = "D6101-40706-60905",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { City = "las vegas", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1996 }

        };

        var result = _policyValidator.TestValidate(policyDTO);
        Assert.True(result.IsValid);
        Assert.DoesNotContain(SFConstants.INVALID_LICENSE_NUMBER, result.Errors.Select(x => x.ErrorCode));
    }

    [Fact]
    [Description("This Unit test, is validating that when Effective Date is less than 30 days in future, then it returns correct Error")]
    public void EffectiveDateShouldBeInvalid()
    {
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Imran",
            LastName = "Khan",
            EffectiveDate = DateTime.Now.AddDays(25),
            LicenseNumber = "D6101-40706-60905",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { City = "las vegas", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1996 }

        };

        var result = _policyValidator.TestValidate(policyDTO);
        Assert.False(result.IsValid);
        Assert.Contains(SFConstants.INVALID_EFFECTIVE_DATE, result.Errors.Select(x => x.ErrorCode));
    }

    [Fact]
    [Description("This Unit test, is validating that when Vehicle Registeration Date is >= 1998, then it returns correct Error")]
    public void VehicleRegisterationYearShouldBeInvalid()
    {
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Imran",
            LastName = "Khan",
            EffectiveDate = DateTime.Now.AddDays(25),
            LicenseNumber = "D6101-40706-60905",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { City = "las vegas", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1998 }

        };

        var result = _policyValidator.TestValidate(policyDTO);
        Assert.False(result.IsValid);
        Assert.Contains(SFConstants.INVALID_VEHICLE_REG_YEAR, result.Errors.Select(x => x.ErrorCode));
    }

    [Fact]
    [Description("This Unit test, is validating that when incorrect US address is provided, then it returns correct Error")]
    public void PolicyUSAddressShouldBeInvalid()
    {
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Imran",
            LastName = "Khan",
            EffectiveDate = DateTime.Now.AddDays(25),
            LicenseNumber = "D6101-40706-60905",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { City = "lahore", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1998 }

        };

        var result = _policyValidator.TestValidate(policyDTO);
        Assert.False(result.IsValid);
        Assert.Contains(SFConstants.INVALID_US_ADDRESS, result.Errors.Select(x => x.ErrorCode));
    }

    [Fact]
    [Description("This Unit test, is validating that when all requirenments are met, then validation resturns no error")]
    public void PolicyDTOShouldBeValid()
    {
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Imran",
            LastName = "Khan",
            EffectiveDate = DateTime.Now.AddDays(31),
            LicenseNumber = "D6101-40706-60905",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { Street = "Building 4, Street 1", City = "las vegas", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1997 }

        };

        var result = _policyValidator.TestValidate(policyDTO);
        Assert.True(result.IsValid);
    }
}
