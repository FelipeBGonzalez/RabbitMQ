using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProdutorRabbitMQ.Domain;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProdutorRabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private ILogger<MailController> _logger;

        public MailController(ILogger<MailController> logger)
        {
            _logger = logger;
        }

        [HttpPost("SendMail")]
        [AllowAnonymous]
        public IActionResult SendMail(Mail mail)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "mailQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                
                string message = JsonSerializer.Serialize(mail);
                var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                        routingKey: "mailQueue",
                        basicProperties: null,
                        body: body);                    
                }


                return Accepted(mail);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Send mail", ex);
                return new StatusCodeResult(500);
            }
        }
    }
}
