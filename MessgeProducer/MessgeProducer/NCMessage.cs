using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCSw.MessgeQueue
{
    /// <summary>
    /// Định dạng gói tin gửi vào Queue
    /// </summary>
    public class NCMessage
    {
        /// <summary>
        /// Loại gói tin
        /// - Dùng để consummer phân biệt gói tin đặng còn xử lý
        /// </summary>
        private MessageType messageType;

        /// <summary>
        /// Phân loại gói tin
        /// - Là 1 chuỗi ký tự để phân loại gói tin
        /// - Đối với dịch vụ công sẽ đẩy luôn Mã thủ tục vào đây để consumer case xử lý
        /// </summary>
        private string messageCategory;

        /// <summary>
        /// Nội dung gói tin dưới dạng chuỗi JSON
        /// </summary>
        private string messageContent;

        public MessageType MessageType { get => messageType; set => messageType = value; }
        public string MessageContent { get => messageContent; set => messageContent = value; }
        public string MessageCategory { get => messageCategory; set => messageCategory = value; }
    }
}
