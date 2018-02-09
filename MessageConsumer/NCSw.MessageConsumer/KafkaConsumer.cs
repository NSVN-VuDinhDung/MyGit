using System.Collections.Generic;
using System.Configuration;
using System.Text;

using Confluent.Kafka;
using Confluent.Kafka.Serialization;


namespace NCSw.MessageConsumer
{


    public class KafkaConsumer : BaseConsumer
    {
        private Consumer<Null, string> kafkaConsumer;

        /// <summary>
        /// Start kafka consumer
        /// </summary>
        public override void StartConsumer()
        {
            var config = new Dictionary<string, object>
                             {
                                 { "group.id", ConfigurationManager.AppSettings["Kafka_GroupID"] },
                                 { "bootstrap.servers", ConfigurationManager.AppSettings["Kafka_Servers"] },
                                 { "enable.auto.commit", "false" }
                             };

            this.kafkaConsumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8));

            var topicName = ConfigurationManager.AppSettings["Kafka_TopicName"];
            this.kafkaConsumer.Subscribe(new string[] { topicName });

            this.kafkaConsumer.OnMessage += (_, msg) =>
                {
                    var message = this.JsonDeserializeObject(msg.Value);
                    this.ProcessMessage(message);
                };

            this.kafkaConsumer.OnError += Consumer_OnError;
            this.kafkaConsumer.OnConsumeError += Consumer_OnConsumeError;

            while (true)
            {
                this.kafkaConsumer.Poll(100);
            }
        }


        /// <summary>
        /// Log lỗi khi consumer bị lỗi trong quá trình get message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consumer_OnConsumeError(object sender, Message e)
        {
            Logger.Error(e.Error.Reason);
        }


        /// <summary>
        /// Log lỗi khi consumer bị lỗi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consumer_OnError(object sender, Error e)
        {
            Logger.Error(e.Reason);
        }

        /// <summary>
        /// Đóng consumer
        /// </summary>
        public override void StopConsumer()
        {
            if (this.kafkaConsumer is null) return;
            this.kafkaConsumer.Unsubscribe();
            this.kafkaConsumer.Unassign();
            this.kafkaConsumer.Dispose();
        }
    }
}
