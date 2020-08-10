using FrameworkBaseService.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkBaseService
{
    public class RabbitListener : IRabbitListener
    {
        ConnectionFactory factory { get; set; }
        IConnection connection { get; set; }
        IModel channel { get; set; }

        public void Register()
        {
            channel.QueueDeclare(queue: "world_dummy", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                int m = 0;
            };
            channel.BasicConsume(queue: "world_dummy", autoAck: true, consumer: consumer);
        }

        public void Deregister()
        {
            this.connection.Close();
        }

        public RabbitListener()
        {
            this.factory = new ConnectionFactory() { HostName = "localhost" };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();

        }
    }
}
