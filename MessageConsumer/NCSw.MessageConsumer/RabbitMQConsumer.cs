using System;
using System.Configuration;
using System.Text;

using RabbitMQ.Client;

namespace NCSw.MessageConsumer
{
    public class RabbitMQConsumer : BaseConsumer
    {
        private IModel rabbitMQConnection;

        /// <summary>
        /// Bắt đầu chạy consumer Rabbit
        /// </summary>
        public override void StartConsumer()
        {
            var sResult = string.Empty;
            var sQueue = ConfigurationManager.AppSettings["RabbitMQ_TopicName"];
            using (var connection = this.GetRabbitConnection())
            {
                this.rabbitMQConnection = connection.CreateModel();
                this.rabbitMQConnection.QueueDeclare(sQueue, true, false, false, null);

                // var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = null;
                while (true)
                {
                    try
                    {
                        result = this.rabbitMQConnection.BasicGet(sQueue, true);
                    }
                    catch (Exception ex)
                    {

                        Logger.Error(ex, "RabbitMQ Get message");
                    }

                    if (result == null) continue;
                    sResult = Encoding.UTF8.GetString(result.Body);
                    this.ProcessMessage(this.JsonDeserializeObject(sResult));

                    // Nghỉ 1 tí cho CPU đỡ mệt nhỉ!
                    // Thread.Sleep(100);
                }

            }
        }

        /// <summary>
        /// Khởi tạo kết nối đến Server RabbitMQ
        /// </summary>
        /// <returns></returns>
        public IConnection GetRabbitConnection()
        {
            var hostName = ConfigurationManager.AppSettings["RabbitMQ_HostName"];
            var port = 5672;
            int.TryParse(ConfigurationManager.AppSettings["RabbitMQ_Port"], out port);
            var userName = ConfigurationManager.AppSettings["RabbitMQ_UserName"];
            var password = ConfigurationManager.AppSettings["RabbitMQ_Password"];

            var connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                ContinuationTimeout = new TimeSpan(0, 0, 0, 30),
                RequestedConnectionTimeout = 30000,
                UserName = userName,
                Password = password
            };
            return connectionFactory.CreateConnection();
        }


        public override void StopConsumer()
        {
            if (this.rabbitMQConnection == null) return;
            this.rabbitMQConnection.Close();
            this.rabbitMQConnection.Dispose();
        }
    }
}
