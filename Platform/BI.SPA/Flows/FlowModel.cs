using BI.SPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA.Flows
{
    /// <summary> 流程相關資訊 </summary>
    public class FlowModel
    {
        /// <summary> 流程種類 </summary>
        public string FlowType { get; set; }

        /// <summary> 流程名稱 </summary>
        public ApprovalLevel Level { get; set; }

        /// <summary> 流程簽核人 </summary>
        public ApprovalRole Role { get; set; }

        /// <summary> 是否為流程起頭 </summary>
        public bool IsStart { get; set; } = false;

        /// <summary> 是否為流程結尾 </summary>
        public bool IsLast { get; set; } = false;

        /// <summary> 為避免改到原資料，複製一份 </summary>
        /// <returns></returns>
        public FlowModel ToCopy()
        {
            return new FlowModel()
            {
                Level = Level,
                Role = Role,
                IsStart = IsStart,
                IsLast = IsLast,
                FlowType = FlowType
            };
        }
    }
}
