using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SF.IP.Application.Common;
using SF.IP.Application.Interfaces.MessageQueue;
using SF.IP.Application.Models;
using SF.IP.Application.Models.InsurancePolicy;
using SF.IP.Domain.Common;
using SF.IP.Domain.DomainEvents;
using SF.IP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SF.IP.Application.Mediators.InsurancyPolicy.EventHandler
{ 

    public class PolicyCreatedEventHandler : INotificationHandler<PolicyCreatedEvent>
    {
        private readonly ILogger<PolicyCreatedEventHandler> _logger;
        private readonly IMQPublisher _mqPublisher;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public PolicyCreatedEventHandler(IMQPublisher mqPublisher, ILogger<PolicyCreatedEventHandler> logger, IMapper mapper , AppSettings appSettings)
        {
            _logger = logger;
            _mqPublisher = mqPublisher;
            _mapper = mapper;
            _appSettings = appSettings;
        }

        public async Task Handle(PolicyCreatedEvent domainEvent, CancellationToken cancellationToken)
        {

            // Accopunting Service
            // State Regulatory Event
            _logger.LogInformation($"Received PolicyCreated Domain Event [{domainEvent.GetType().Name}] - [{domainEvent.Policy}]");

            var insurancePolicyDTO = _mapper.Map<InsurancePolicy, InsurancePolicyDTO>(domainEvent.Policy);

            _logger.LogInformation($"Sending PolicyCreatedEvent [{domainEvent.Policy}] to Accounting Queue");
            var isInjested = await _mqPublisher.PublishAsync<InsurancePolicyDTO>(_appSettings.RabbitMQ.QueueExchange,
                _appSettings.RabbitMQ.AccountingQueue, insurancePolicyDTO, "");

            _logger.LogInformation($"Sending PolicyCreatedEvent [{domainEvent.Policy}] to State Regulatory Queue");
            isInjested = await _mqPublisher.PublishAsync<InsurancePolicyDTO>(_appSettings.RabbitMQ.QueueExchange,
                _appSettings.RabbitMQ.StateRegulatoryQueue, insurancePolicyDTO, "");

            return ;
        }
    }
}
