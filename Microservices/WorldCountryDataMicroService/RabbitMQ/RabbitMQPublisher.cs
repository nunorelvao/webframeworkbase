using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCountryDataMicroService.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        string hostName = "localhost";
        ConnectionFactory factory;
        public RabbitMQPublisher()
        {
            factory = new ConnectionFactory() { HostName = hostName };
        }
        public Task SendMessage(string qName, object message)
        {
            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: qName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;


                    var body = Encoding.UTF8.GetBytes(message.ToString());

                    channel.BasicPublish(exchange: "",
                                         routingKey: qName,
                                         basicProperties: properties,
                                         body: body);

                }

            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }

            return Task.CompletedTask;
        }

    }
}
