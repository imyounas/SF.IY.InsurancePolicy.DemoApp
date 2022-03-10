using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using SF.IP.Application.Interfaces.Database;
using SF.IP.Application.Interfaces.StateRegulation;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Application.Models.InsurancePolicy.Result;
using SF.IP.Application.Validators.PolicyInsurance;
using SF.IP.Domain.DomainEvents;
using SF.IP.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SF.IP.Application.Mediators.InsurancyPolicy.Command;

public class CreatePolicyCommand : IRequest<CreatePolicyResultDTO>
{
    public InsurancePolicyDTO InsurancePolicy { get; set; }
}

public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, CreatePolicyResultDTO>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePolicyCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IPolicyStateRegulator _regulator;
    public CreatePolicyCommandHandler(ILogger<CreatePolicyCommandHandler> logger, IApplicationDbContext dbContext, IMapper mapper, IMediator mediator, IPolicyStateRegulator regulator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
        _regulator = regulator;
    }

    public async Task<CreatePolicyResultDTO> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Creatiing Insurance Policy - [{request.InsurancePolicy.ToString()}]");

        ValidationResult modelResults = new PolicyValidator(_dbContext).Validate(request.InsurancePolicy);

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

        // Validate the policy with state regulator before creation
        // here I would suggest my ProductOwner/Client to not to make this call Synchrnous, but show the Policy status as 'Pending' to client
        // and in the result of async call or background job to get the regulator status for all new Policies, then update the Web UI with SingalR or Web Sockets etc.
        var regResponse = await _regulator.ValidatePolicyFromStateRegulator(request.InsurancePolicy);

        if (!regResponse.Status)
        {
            _logger.LogDebug($"Insurance Policy - [{request.InsurancePolicy}] has been declined by State Regulation => [{regResponse.Reason}]");
            // this will cause HTTPStatusCode 500
            // in real application, in case of custom exceptions, we can send other more appropriate http status codes
            throw new Exception($"Insurance Policy - [{request.InsurancePolicy}] has been declined by State Regulation => [{regResponse.Reason}]");
        }
        else
        {
            _logger.LogDebug($"Insurance Policy - [{request.InsurancePolicy}] has been approved by State Regulation => [{regResponse.Reason}]");
        }

        _dbContext.InsurancePolicies.Add(policy);
        int rowsUpdated = await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogDebug($"Insurance Policy - [{request.InsurancePolicy}] Successfully Created. Rows Updated: [{rowsUpdated}]");

        _logger.LogDebug($"Firing Insurance Policy - [{request.InsurancePolicy}] Created Event");

        // not waiting for the async call to fire events
        // though we can await here as well, cause in the evnt handler I am sending these events to RabbitMQ Queues,
        // so the Queue processor could do the heavy lifting against these events
        _mediator.Publish(new PolicyCreatedEvent(policy)).ConfigureAwait(false);

        result.IsSuccesfull = true;
        result.PolicyId = policy.Id.ToString();

        return result;
    }
}

