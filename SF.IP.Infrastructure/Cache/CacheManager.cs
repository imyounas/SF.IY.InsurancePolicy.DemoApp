using SF.IP.Application.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Infrastructure.Cache
{
    public class CacheManager : ICacheManager
    {
        public Guid ValidateTokenAndSendClientId(string serverToken)
        {
            // check server token and return clientId
            return Guid.NewGuid();
        }

        public bool VerifySender(Guid clientId, string sender)
        {
            // check cache for valid sender against clientId
            return true;
        }
    }
}
