using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using Confluent.Kafka;
using Confluent.Kafka.Serialization;

using NCSw.MessgeQueue;

using Newtonsoft.Json;

using NLog;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NCSw.MessageConsumer
{
    /// <inheritdoc />
    /// <summary>
    /// The message consumer.
    /// </summary>
    public partial class MessageConsumer : ServiceBase
    {
        #region Variable

        private string m_serverType = "KAFKA";

        private BaseConsumer consumer;

        #endregion

        public MessageConsumer()
        {
            this.InitializeComponent();
        }

        #region Window Service

        protected override void OnStart(string[] args)
        {
            try
            {
                this.StartConsumer();

            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex, "On Start");
            }
        }

        private void StartConsumer()
        {
            this.m_serverType = ConfigurationManager.AppSettings["Message_ServerType"];

            switch (this.m_serverType.ToUpper())
            {
                case "KAFKA":
                    this.consumer = new KafkaConsumer();
                    break;
                case "RABBITMQ":
                    this.consumer = new RabbitMQConsumer();
                    break;
                default:
                    this.consumer = new KafkaConsumer();
                    break;
            }

            this.consumer.StartConsumer();
        }

        protected override void OnStop()
        {
            try
            {
                this.consumer.StopConsumer();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex, "On Start");
            }
        }

        #endregion


    }
}
