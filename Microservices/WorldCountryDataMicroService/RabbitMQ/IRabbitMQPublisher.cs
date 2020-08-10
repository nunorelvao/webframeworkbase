using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCountryDataMicroService.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        Task SendMessage(string qName, object message);

    }
}
