using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using WorldCountryDataMicroService.Models;

namespace RabbitMQTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQTest : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.QueueDeclare(queue: "hello",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: properties,
                                     body: body);

            }


            return "OK Queue [hello] sent to RabbitMQ with message [Hello World!]";
        }

    }
}
