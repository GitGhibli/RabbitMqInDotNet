using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                //In the .NET client, when we supply no parameters to QueueDeclare() we create a non-durable, exclusive, autodelete queue with a generated name:
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName, "logs", routingKey: "");
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

                Console.WriteLine("Waiting for logs");
                Console.ReadLine();
            }

        }
    }
}
