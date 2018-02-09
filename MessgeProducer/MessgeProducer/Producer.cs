using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System.Configuration;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace NCSw.MessgeQueue
{
    public class Producer
    {

        private string m_serverType = "KAFKA";

        /// <summary>
        /// Gửi message vào Queue
        /// </summary>
        /// <param name="topicName">Tên topic</param>
        /// <param name="message">Nội dung message</param>
        public void SendMessage(string topicName, string message)
        {
            m_serverType = ConfigurationManager.AppSettings["Message_ServerType"];

            switch (m_serverType.ToUpper())
            {
                case "KAFKA":
                    SendMessageToKafka(topicName, message);
                    break;
                case "RABBITMQ":
                    SendMessageToRabbitMQ(topicName, message);
                    break;
                default:
                    SendMessageToKafka(topicName, message);
                    break;
            }
        }

        #region Kafka


        /// <summary>
        /// Gửi Message vào Kafka
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Result SendMessageToKafka(string topicName, string message)
        {
            var result = Result.False;
            var config = new Dictionary<string, object>
              {
                { "bootstrap.servers", ConfigurationManager.AppSettings["Kafka_BrokerList"] }
              };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                producer.ProduceAsync(topicName, null, message);

                producer.Flush(100);
                producer.OnError += Producer_OnError;

                producer.Dispose();

                result = Result.Success;
            }

            return result;
        }

        /// <summary>
        /// Đẩy lỗi về app chính để ghi Log
        /// </summary>
        private void Producer_OnError(object sender, Error e)
        {
            throw new Exception(e.Reason);
        }

        #endregion

        #region RabbitMQ

        /// <summary>
        /// Gửi message vào RabbitMQ
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Result SendMessageToRabbitMQ(string topicName, string message)
        {
            var result = Result.False;

            IBasicProperties props = new BasicProperties()
            {
                Persistent = true
            };


            using (var connection = InitRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(topicName, true, false, false, null);

                    channel.BasicPublish(string.Empty, topicName, props, Encoding.UTF8.GetBytes(message));
                    channel.Close();

                    result = Result.Success;
                }
            }

            return result;
        }

        /// <summary>
        /// Khởi tạo kết nối vào RabbitMQ
        /// </summary>
        /// <returns></returns>
        private IConnection InitRabbitMQConnection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["RabbitMQ_HostName"],
                Port = int.Parse(ConfigurationManager.AppSettings["RabbitMQ_Port"]),
                //ContinuationTimeout = new TimeSpan(0, 0, 0, 30),
                RequestedConnectionTimeout = int.Parse(ConfigurationManager.AppSettings["RabbitMQ_ConnectionTimeOut"]),
                UserName = ConfigurationManager.AppSettings["RabbitMQ_UserName"],
                Password = ConfigurationManager.AppSettings["RabbitMQ_Password"]
            };
            return connectionFactory.CreateConnection();
        }

        #endregion

    }
}
