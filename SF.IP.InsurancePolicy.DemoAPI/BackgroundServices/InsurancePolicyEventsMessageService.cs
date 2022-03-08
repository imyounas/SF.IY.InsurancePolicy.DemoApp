using SF.IP.Application.Common;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SF.IP.Application.Models.InsurancePolicy;

namespace PM.IY.EmailRouterDemoApp.BackgroundServices
{
    /// <summary>
    /// should be separate hosted service to consume the messages and route them based on their type and meta data
    /// but for the sake of brevity adding into same project and only consuming Accounting Queue messages. 
    /// Same can be done for other Queues.
    /// </summary>
    public class InsurancePolicyEventsMessageService : BackgroundService
    {

        private readonly IModel _channel;
        private readonly DefaultObjectPool<IModel> _mqConnectionPool;
        private readonly AppSettings _appSettings;
        private readonly string _exchangeType;
        private readonly ISender _mediator;
        private readonly ILogger<InsurancePolicyEventsMessageService> _logger;

        public InsurancePolicyEventsMessageService(ISender mediator, IPooledObjectPolicy<IModel> objectPolicy, AppSettings appSettings, ILogger<InsurancePolicyEventsMessageService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
            _mqConnectionPool = new DefaultObjectPool<IModel>(objectPolicy, SF.IP.Application.Common.Constants.MAX_RETAINED_MQ_CONNECTIONS);
            _channel = _mqConnectionPool.Get();
            _exchangeType = ExchangeType.Topic;
            _mediator = mediator;

            Initialize(_appSettings.RabbitMQ.QueueExchange, _exchangeType, _appSettings.RabbitMQ.AccountingQueue, _appSettings.RabbitMQ.AccountingQueue);
        }

        public void Initialize(string exchangeName, string exchangeType, string queueName, string routingKey)
        {
            try
            {
                if (_exchangeType == ExchangeType.Direct)
                {
                    routingKey = queueName;
                }

                _channel.ExchangeDeclare(exchange: exchangeName,
                                 type: exchangeType,
                                 durable: true,
                                 autoDelete: false,
                                 arguments: null);

                _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                _channel.QueueBind(queueName, exchangeName, routingKey, null);
                _channel.BasicQos(0, 1, false);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Initializing in InsurancePolicyEventsMessageService. Error [{ex.Message}]");
            }

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);

            string content = "";
            consumer.Received += (ch, e) =>
            {
                content = Encoding.UTF8.GetString(e.Body.ToArray());
                var emailRequestMessage = JsonConvert.DeserializeObject<InsurancePolicyDTO>(content);

                _channel.BasicAck(e.DeliveryTag, false);
            };

            consumer.Shutdown += Consumer_Shutdown; ;
            consumer.Registered += Consumer_Registered; ;
            consumer.Unregistered += Consumer_Unregistered; ;
            consumer.ConsumerCancelled += Consumer_ConsumerCancelled; ;

            _channel.BasicConsume(_appSettings.RabbitMQ.AccountingQueue, false, consumer);

            return Task.CompletedTask;
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
