using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Messages.Enums
{
    /// <summary> 信件緊急程度 </summary>
    public enum MailPriorityEnum : byte
    {
        /// <summary> 一般 </summary>
        Default = 1,

        /// <summary> 高 </summary>
        High = 2,

        /// <summary> 最高 </summary>
        Critical = 3,
    }
}
