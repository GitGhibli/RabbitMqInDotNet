using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher
{
    class Publisher
    {
        static void Main()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);

                var message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);

                Console.WriteLine("message sent");
            }
        }
    }
}
