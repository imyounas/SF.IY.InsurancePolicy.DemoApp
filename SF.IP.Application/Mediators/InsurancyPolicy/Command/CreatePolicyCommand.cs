using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using SF.IP.Application.Interfaces.Database;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Application.Models.InsurancePolicy.Result;
using SF.IP.Application.Validators.PolicyInsurance;
using SF.IP.Domain.DomainEvents;
using SF.IP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SF.IP.Application.Mediators.InsurancyPolicy.Command
{
    public class CreatePolicyCommand : IRequest<CreatePolicyResultDTO>
    {
        public InsurancePolicyDTO InsurancePolicy { get; set; }
    }

    public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, CreatePolicyResultDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePolicyCommandHandler> _logger;
        private readonly IMediator? _mediator;
        public CreatePolicyCommandHandler(ILogger<CreatePolicyCommandHandler> logger, IApplicationDbContext dbContext, IMapper mapper , IMediator? mediator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<CreatePolicyResultDTO> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Creatiing Insurance Policy - [{request.InsurancePolicy.ToString()}]");

            ValidationResult modelResults = new PolicyValidator().Validate(request.InsurancePolicy);

            CreatePolicyResultDTO result = new CreatePolicyResultDTO();

            if (!modelResults.IsValid)
            {
                result.IsSuccesfull = false;
                foreach (var failure in modelResults.Errors)
                {
                    result.Errors.Add(failure.ErrorMessage);
                }

                return result;
            }

            var policy = _mapper.Map<InsurancePolicyDTO, InsurancePolicy>(request.InsurancePolicy);
            
             _dbContext.InsurancePolicies.Add(policy);
            int rowsUpdated = await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogDebug($"Insurance Policy - [{request.InsurancePolicy}] Successfully Created. Rows Updated: [{rowsUpdated}]");

            _logger.LogDebug($"Firing Insurance Policy - [{request.InsurancePolicy}] Created Event");
            await _mediator.Publish(new PolicyCreatedEvent(policy)).ConfigureAwait(false);

            result.IsSuccesfull = true;
            result.PolicyId = policy.Id;

            return result;
        }
    }
}
