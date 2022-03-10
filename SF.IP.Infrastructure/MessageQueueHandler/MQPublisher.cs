using SF.IP.Application.Interfaces.MessageQueue;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Infrastructure.MessageQueueHandler;

public class MQPublisher : IMQPublisher
{
    private readonly DefaultObjectPool<IModel> _mqConnectionPool;
    private readonly ILogger<RabbitMQPooledObjectPolicy> _logger;
    private readonly string _exchangeType;
    public MQPublisher(IPooledObjectPolicy<IModel> objectPolicy, ILogger<RabbitMQPooledObjectPolicy> logger)
    {
        _logger = logger;
        _mqConnectionPool = new DefaultObjectPool<IModel>(objectPolicy, Application.Common.SFConstants.MAX_RETAINED_MQ_CONNECTIONS);
        _exchangeType = ExchangeType.Direct;
    }

    public async Task<bool> PublishAsync<T>(string exchangeName, string queueName, T message, string routingKey)
    {
        bool status = true;

        if (message == null)
        {
            status = false;
            return status;
        }

        var _channel = _mqConnectionPool.Get();

        try
        {
            if (_exchangeType == ExchangeType.Direct)
            {
                routingKey = queueName;
            }

            _channel.ExchangeDeclare(exchange: exchangeName,
                                type: _exchangeType,
                                durable: true,
                                autoDelete: false,
                                arguments: null);

            _channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

            _channel.QueueBind(queue: queueName,
                             exchange: exchangeName,
                             routingKey: routingKey);

            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = Guid.NewGuid().ToString();
            properties.Type = message.GetType().FullName;

            _channel.BasicPublish(exchange: exchangeName,
                                routingKey: routingKey,
                                basicProperties: properties,
                                body: messageBody);

        }

        catch (Exception ex)
        {
            _logger.LogError($"Some error while publishing message to [{queueName}]. Error: [{ex.Message}]");
            status = false;
        }
        finally
        {
            _mqConnectionPool.Return(_channel);
        }

        return await Task.FromResult(status);
    }
}
