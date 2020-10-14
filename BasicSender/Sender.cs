using RabbitMQ.Client;
using System;
using System.Text;

namespace BasicSender
{
    class Sender
    {
        static void Main()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);

                Console.WriteLine("message sent");
            }

            Console.ReadLine();
        }
    }
}