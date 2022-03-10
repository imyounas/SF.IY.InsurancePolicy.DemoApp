using SF.IP.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using SF.IP.Application.Models.InsurancePolicy.Request;
using SF.IP.Application.Mediators.InsurancyPolicy.Command;
using SF.IP.Application.Mediators.InsurancyPolicy.Query;

namespace SF.IY.InsurancePolicy.DemoAPI.Controllers;

public class IPController : APIBaseController
{
    private readonly AppSettings _appSettings;
    private readonly ILogger<IPController> _logger;

    public IPController(AppSettings appSettings, ILogger<IPController> logger)
    {
        _appSettings = appSettings;
        _logger = logger;
    }


    [HttpPost]
    [Route("CreatePolicy")]
    public async Task<ActionResult> CreatePolicy([FromBody] CreatePolicyRequestDTO policyRequest)
    {
        _logger.LogDebug($"Received Policy Creation Request [{policyRequest.RequestId}] - [{policyRequest.InsurancePolicy}]");

        try
        {

            var command = new CreatePolicyCommand
            {
                InsurancePolicy = policyRequest.InsurancePolicy
            };

            var res = await Mediator.Send(command);

            return res.IsSuccesfull ? Ok(res) : BadRequest(res);
        }
        // in real application we should have custome exception types suited for our business domain
        catch (Exception ex)
        {
            _logger.LogError($"Error while processing Policy Creation Request : [{ex.Message}]");
            return Problem($"Error while processing Policy Creation Request : [{ex.Message}]");
        }
    }

    [HttpGet]
    [Route("PolicyById")]
    public async Task<ActionResult> GetPolicyById([FromQuery] GetPolicyByIdRequestDTO policyRequest)
    {
        _logger.LogDebug($"Received Get Policy by Id Request [{policyRequest.RequestId}] - [{policyRequest.PolicyId}]");

        try
        {

            var query = new GetPolicyByIdQuery
            {
                PolicyId = Guid.Parse(policyRequest.PolicyId),
                LicenseNumber = policyRequest.LicenseNumber
            };

            var res = await Mediator.Send(query);

            return res.IsSuccesfull ? Ok(res) : NotFound(res);
        }
        // in real application we should have custome exception types suited for our business domain
        catch (Exception ex)
        {
            _logger.LogError($"Error while processing Get Policy by Id [{policyRequest.PolicyId}] Request : [{ex.Message}]");
            return Problem($"Error while processing Get Policy by Id [{policyRequest.PolicyId}] Request : [{ex.Message}]");
        }
    }

    [HttpGet]
    [Route("PolicyByLicenseNumber")]
    public async Task<ActionResult> GetPolicyByLicenseNumber([FromQuery] GetPoliciesByLicenseRequestDTO policyRequest)
    {
        _logger.LogDebug($"Received Get Policy by LicenseNumber [{policyRequest.RequestId}] - [{policyRequest.LicenseNumber}]");

        try
        {

            var query = new GetPoliciesByDrivingLicenseQuery
            {
                LicenseNumber = policyRequest.LicenseNumber,
                IncludeExpiredPolicies = policyRequest.IncludeExpiredPolicies,
                SortAscByVehicleRegisterationYear = policyRequest.SortAscByVehicleRegisterationYear
            };

            var res = await Mediator.Send(query);

            return res.IsSuccesfull ? Ok(res) : NotFound(res);
        }
        // in real application we should have custome exception types suited for our business domain
        catch (Exception ex)
        {
            _logger.LogError($"Error while processing Get Policy by License Number [{policyRequest.LicenseNumber}] Request : [{ex.Message}]");
            return Problem($"Error while processing Get Policy by License Number [{policyRequest.LicenseNumber}] Request : [{ex.Message}]");
        }
    }

}

