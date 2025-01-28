using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Messages.Enums
{
    /// <summary> 信件寄送狀態 </summary>
    public enum MailStatusEnum : byte
    {
        /// <summary> 剛建立 </summary>
        Default = 0,

        /// <summary> 失敗 </summary>
        Fail = 1,

        /// <summary> 寄送完成 </summary>
        Complete = 2,
    }
}
