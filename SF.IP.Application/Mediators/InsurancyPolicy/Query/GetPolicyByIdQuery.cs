using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SF.IP.Application.Interfaces.Database;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Application.Models.InsurancePolicy.Result;
using SF.IP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SF.IP.Application.Mediators.InsurancyPolicy.Query
{
    public class GetPolicyByIdQuery : IRequest<GetPolicyByIdResultDTO>
    {
        public Guid PolicyId { get; set; }
        public string LicenseNumber { get; set; }
    }

    public class GetPolicyByIdQueryHandler : IRequestHandler<GetPolicyByIdQuery, GetPolicyByIdResultDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPolicyByIdQueryHandler> _logger;
        public GetPolicyByIdQueryHandler(ILogger<GetPolicyByIdQueryHandler> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetPolicyByIdResultDTO> Handle(GetPolicyByIdQuery request, CancellationToken cancellationToken)
        {

            // here, if we like , can first try validating the format of license. 
            GetPolicyByIdResultDTO result = new GetPolicyByIdResultDTO();

            // in cases where we could have complex query filter criteria, with optional inclusion exclusion of resultant columns
            // it is better to use Specification pattern with proper repositories. 

            if (string.IsNullOrWhiteSpace(request.PolicyId.ToString()) || request.PolicyId == Guid.Empty)
            {
                result.IsSuccesfull = false;
                result.Errors.Add("Invalid PolicyId");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.LicenseNumber) || !Regex.Match(request.LicenseNumber, Common.SFConstants.LICENSE_REGEX, RegexOptions.IgnoreCase).Success)
            {
                result.IsSuccesfull = false;
                result.Errors.Add("Invalid License Number");
                return result;
            }

            // I think here for this Query, License number requirenment has been added by mistake ??
            _logger.LogDebug($"Getting Insurance Policy by Id [{request.PolicyId}] & License Number [{request.LicenseNumber}]");
            var policy = _dbContext.InsurancePolicies.FirstOrDefault(p => p.Id == request.PolicyId && p.LicenseNumber.Equals(request.LicenseNumber, StringComparison.OrdinalIgnoreCase));

            if(policy == null)
            {
                _logger.LogDebug($"No Insurance Policy found by Id [{request.PolicyId}]");
                result.IsSuccesfull = false;
                return result;
            }

            _logger.LogDebug($"Returning Insurance Policy by Id [{request.PolicyId}]");

            result.InsurancePolicy = _mapper.Map<InsurancePolicy, InsurancePolicyDTO>(policy);
            result.IsSuccesfull = true;

            return await Task.FromResult(result); ;

        }
    }
}
