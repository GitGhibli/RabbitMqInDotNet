using RabbitMQ.Client;
using System;
using System.Text;

namespace NewTask
{
    class NewTask
    {
        static void Main(string[] args)
        {
            var message = args.Length > 0 ? string.Join(" ", args) : "no arguments";

            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("tasks", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "tasks", basicProperties: null, body: body);

                Console.WriteLine("message sent");
            }
        }
    }
}
