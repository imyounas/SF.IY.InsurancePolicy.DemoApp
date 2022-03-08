using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Common
{
    public class AppSettings
    {
        public AppSettings()
        {
            ConnectionStrings = new ConnectionStrings();
            RabbitMQ = new RabbitMQ();
        }

        public bool UseInMemoryDatabase { get; set; }
        public string InMemoryDatabaseName { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public RabbitMQ RabbitMQ { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
    public class RabbitMQ
    {
        public string QueueExchange { get; set; }
        public string QueueHost { get; set; }
        public string QueueUserName { get; set; }
        public string QueuePassword { get; set; }
        public int QueuePort { get; set; }
        public string AccountingQueue { get; set; }
        public string StateRegulatoryQueue { get; set; }
    }
}
