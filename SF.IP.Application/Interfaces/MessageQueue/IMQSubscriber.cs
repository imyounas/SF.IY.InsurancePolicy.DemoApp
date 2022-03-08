using System;
using System.Threading.Tasks;

namespace SF.IP.Application.Interfaces.MessageQueue
{
    public interface IMQSubscriber
    {
        void SubscribeAsync<T>(Func<T, Task<bool>> callback);
    }
}
