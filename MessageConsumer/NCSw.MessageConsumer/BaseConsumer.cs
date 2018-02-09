using System;


namespace NCSw.MessageConsumer
{
    using NCSw.MessgeQueue;

    using Newtonsoft.Json;

    using NLog;

    public class BaseConsumer : IConsumer
    {
        protected static Logger Logger
        {
            get
            {
                return m_logger;
            }
            set
            {
                m_logger = value;
            }
        } 
        
        private static Logger m_logger;

        public BaseConsumer()
        {
            m_logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Xử lý gói tin lấy được từ Queue
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected string ProcessMessage(NCMessage message)
        {

            switch (message.MessageType)
            {
                case MessageType.DVC:
                    // TODO: 
                    // Gọi đến các API XL dịch vụ công
                    Console.WriteLine(message.MessageContent);
                    break;
                case MessageType.Log:
                    // TODO:
                    // Gọi đến API XL lỗi
                    Console.WriteLine(message.MessageContent);
                    break;
                default:
                    Console.WriteLine(message.MessageContent);
                    break;
            }

            return message.MessageContent;
        }

        public virtual void StartConsumer()
        {
            throw new NotImplementedException();
        }

        public virtual void StopConsumer()
        {
            throw new NotImplementedException();
        }

        #region Other functions


        /// <summary>
        /// Deserialize gói tin từ chuỗi sang object
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public NCMessage JsonDeserializeObject(string jsonString)
        {
            return JsonConvert.DeserializeObject<NCMessage>(
                jsonString,
                new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                });
        }


        #endregion

    }
}
