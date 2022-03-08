using System.Threading.Tasks;

namespace SF.IP.Application.Interfaces.MessageQueue
{
    public interface IMQPublisher
    {
        /*
        * Publish Message to Queue
        */
        Task<bool> PublishAsync<T>(string exchangeName, string queueName, T message, string routingKey);
    }
}
