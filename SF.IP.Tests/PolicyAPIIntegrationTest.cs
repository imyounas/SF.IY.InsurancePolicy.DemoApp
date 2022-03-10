using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Application.Models.InsurancePolicy.Request;
using System;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SF.IP.Tests;

public class PolicyAPIIntegrationTest : BaseServiceUnitTest, IClassFixture<WebApplicationFactory<SF.IY.InsurancePolicy.DemoAPI.Startup>>
{
    private readonly WebApplicationFactory<SF.IY.InsurancePolicy.DemoAPI.Startup> _factory;

    public PolicyAPIIntegrationTest(WebApplicationFactory<SF.IY.InsurancePolicy.DemoAPI.Startup> factory)
    {
        _factory = factory;
        SetUpPolicyUSZipCodes();
    }

    [Fact]
    [Description("This integration test, is validating the successful creation of Insurance Policy using IPController POST method")]
    public async Task PolicyCreationAPITest_Passing()
    {
        var client = _factory.CreateClient();
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

        CreatePolicyRequestDTO request = new CreatePolicyRequestDTO() { InsurancePolicy = policyDTO, RequestId = "123" };

        var response = await client.PostAsJsonAsync("api/IP/CreatePolicy", request);

        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Contains("application/json", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    [Description("This integration test, is testing that when State Regulation validation fails then API returns HTTP (500) status")]
    public async Task PolicyCreationAPITest_FailingDueToStateRegulations()
    {
        var client = _factory.CreateClient();
        var policyDTO = new InsurancePolicyDTO()
        {
            FirstName = "Trump",
            LastName = "Khan",
            EffectiveDate = DateTime.Now.AddDays(31),
            LicenseNumber = "D6101-40706-60905",
            ExpirationDate = DateTime.Now.AddDays(365),
            Address = new AddressDTO() { Street = "Building 4, Street 1", City = "las vegas", State = "nevada", ZipCode = "89144" },
            PremiumPrice = new PremiumPriceDTO() { Currency = "$", Price = 100.0 },
            VehicleDetail = new VehicleDTO() { Manufacturer = "Honda", Model = "Civic", Name = "EagleEyes", Year = 1997 }

        };

        CreatePolicyRequestDTO request = new CreatePolicyRequestDTO() { InsurancePolicy = policyDTO, RequestId = "123" };

        var response = await client.PostAsJsonAsync("api/IP/CreatePolicy", request);
        int actualCode = (int)response.StatusCode;
        int expectedCode = (int)System.Net.HttpStatusCode.InternalServerError;

        Assert.Equal(expectedCode, actualCode);
        Assert.Contains("application/problem+json", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    [Description("This integration test, is validating that API is returning Policy against a correct License Number")]
    public async Task GetPolicyByLicenseNumberAPITest_Passing()
    {
        var client = _factory.CreateClient();
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

        CreatePolicyRequestDTO request = new CreatePolicyRequestDTO() { InsurancePolicy = policyDTO, RequestId = "123" };

        var response = await client.PostAsJsonAsync("api/IP/CreatePolicy", request);
        response.EnsureSuccessStatusCode();

        var getPolicyResponse = await client.GetAsync($"api/IP/PolicyByLicenseNumber?LicenseNumber={policyDTO.LicenseNumber}&SortAscByVehicleRegisterationYear=true");

        getPolicyResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Contains("application/json", getPolicyResponse.Content.Headers.ContentType.ToString());
    }

    [Fact]
    [Description("This integration test, is validating that API is not returning Policy when incorrect License Number is provided")]
    public async Task GetPolicyByLicenseNumberAPITest_Failing()
    {
        var client = _factory.CreateClient();

        var getPolicyResponse = await client.GetAsync($"api/IP/PolicyByLicenseNumber?LicenseNumber=D7777-88706-60905&SortAscByVehicleRegisterationYear=true");

        Assert.False(getPolicyResponse.IsSuccessStatusCode);
        Assert.Contains("application/json", getPolicyResponse.Content.Headers.ContentType.ToString());
    }
}
