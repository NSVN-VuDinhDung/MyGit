using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCSw.MessageConsumer
{
    /// <summary>
    /// Consumer Interface
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// Bắt đầu chạy consumer
        /// </summary>
        void StartConsumer();


        /// <summary>
        /// Dừng Consumer
        /// </summary>
        void StopConsumer();
    }
}
