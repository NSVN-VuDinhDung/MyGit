using NCSw.MessageConsumer;

namespace TestConsumer
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The program.
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            BaseConsumer consumer = new KafkaConsumer();
            consumer.StartConsumer();
        }
    }
}
