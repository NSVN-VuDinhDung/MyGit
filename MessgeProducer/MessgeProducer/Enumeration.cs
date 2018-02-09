using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCSw.MessgeQueue
{
    /// <summary>
    /// Loại Message để phân biệt Log với nghiệp vụ
    /// </summary>
    public enum MessageType
    {
        DVC = 1,
        Log = 2
    }


    /// <summary>
    /// Kết quả add message vào queue
    /// </summary>
    public enum Result
    {
        False = 0,
        Success = 1
    }

}
