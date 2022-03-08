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
    public class GetPoliciesByDrivingLicenseQuery : IRequest<GetPoliciesByLicenseResultDTO>
    {
        public string LicenseNumber { get; set; }
        public bool SortAscByVehicleRegisterationYear { get; set; }
        public bool IncludeExpiredPolicies { get; set; }
    }

    public class GetPoliciesByDrivingLicenseQueryHandler : IRequestHandler<GetPoliciesByDrivingLicenseQuery, GetPoliciesByLicenseResultDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPoliciesByDrivingLicenseQueryHandler> _logger;
        public GetPoliciesByDrivingLicenseQueryHandler(ILogger<GetPoliciesByDrivingLicenseQueryHandler> logger, IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetPoliciesByLicenseResultDTO> Handle(GetPoliciesByDrivingLicenseQuery request, CancellationToken cancellationToken)
        {

            // here, if we like , can first try validating the format of license. 
            GetPoliciesByLicenseResultDTO result = new GetPoliciesByLicenseResultDTO();

            // in cases where we could have complex query filter criteria, with optional inclusion exclusion of resultant columns
            // it is better to use Specification pattern with proper repositories. 

            if (string.IsNullOrWhiteSpace(request.LicenseNumber) || !Regex.Match(request.LicenseNumber, Common.Constants.LICENSE_REGEX, RegexOptions.IgnoreCase).Success)
            {
                result.IsSuccesfull = false;
                result.Errors.Add("Invalid License Number");
                return result;
            }

            _logger.LogDebug($"Getting Insurance Policy by License Number [{request.LicenseNumber}]");
            var policyQuery = _dbContext.InsurancePolicies.Where(p => p.LicenseNumber.Equals(request.LicenseNumber, StringComparison.OrdinalIgnoreCase));

            if (!request.IncludeExpiredPolicies)
            {
                policyQuery = policyQuery.Where(p => p.ExpirationDate > DateTime.UtcNow);
            }

            policyQuery = request.SortAscByVehicleRegisterationYear ? policyQuery.OrderBy(p => p.VehicleDetail.Year)
                                                                    : policyQuery.OrderByDescending(p => p.VehicleDetail.Year);


            var policies = policyQuery.ToList();

            _logger.LogDebug($"Returning [{policies.Count}] Insurance Policy by License Number [{request.LicenseNumber}]");

            result.InsurancePolicies = _mapper.Map<List<InsurancePolicy>, List<InsurancePolicyDTO>>(policies);

            result.IsSuccesfull = true;

            return result;

        }
    }
}
