using ConsumidorRabbitMQ.Application;
using ConsumidorRabbitMQ.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace ConsumidorRabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            SendEmail sendEmail = new SendEmail();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "mailQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var mail = JsonSerializer.Deserialize<Mail>(message);
                        sendEmail.Excexute(mail);
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, false);
                    }

                };                

                channel.BasicConsume(queue: "mailQueue",
                    autoAck: false,
                    consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();

            }

        }       
    }
}
