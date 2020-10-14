using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace WorkQueue
{
    class WorkQueue
    {
        static void Main()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Thread.Sleep(1000 * message.Split(".").Length - 1);
                    
                    Console.WriteLine(" [x] Received {0}", message);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: "tasks",
                                 autoAck: true,
                                 consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
