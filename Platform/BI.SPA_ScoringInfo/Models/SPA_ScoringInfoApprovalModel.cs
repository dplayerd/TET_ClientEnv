using BI.SPA_ScoringInfo.Utils;
using BI.SPA_ScoringInfo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models
{
    public class SPA_ScoringInfoApprovalModel
    {
        /// <summary> 系統辨識碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 評鑑計分資料系統辨識碼 </summary>
        public Guid SIID { get; set; }

        /// <summary> 審核類型 </summary>
        public string Type { get; set; }

        /// <summary> 審核說明 </summary>
        public string Description { get; set; }

        /// <summary> 審核關卡 </summary>
        public string Level { get; set; }

        /// <summary> 審核者 </summary>
        public string Approver { get; set; }

        /// <summary> 審核結果 </summary>
        public string Result { get; set; }

        /// <summary> 審核意見 </summary>
        public string Comment { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }


        #region 其它欄位
        /// <summary> 顯示用審核關卡名稱 </summary>
        public string Level_Text
        {
            get
            {
                var lvl = ApprovalUtils.ParseApprovalLevel(this.Level);
                return lvl.ToDisplayText();
            }
        }

        /// <summary> 審核ID </summary>
        public Guid ApprovalID { get { return this.ID; } set { this.ID = value; } }

        /// <summary> 審核意見 </summary>
        public string ApprovalComment { get { return this.Comment; } set { this.Comment = value; } }

        /// <summary> CreateDate </summary>
        public string CreateDate_Text { get { return this.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"); } }

        /// <summary> ModifyDate </summary>
        public string ModifyDate_Text { get { return this.ModifyDate.ToString("yyyy-MM-dd HH:mm:ss"); } }
        #endregion
    }
}
