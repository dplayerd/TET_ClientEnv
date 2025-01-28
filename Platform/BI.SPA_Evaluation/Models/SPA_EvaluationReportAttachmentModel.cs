using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Evaluation.Models
{
    /// <summary> SPA 績效評鑑報告附件 </summary>
    public class SPA_EvaluationReportAttachmentModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 績效評鑑報告系統辨識碼 </summary>
        public Guid EPID { get; set; }

        /// <summary> 檔案分類 </summary>
        public string FileCategory { get; set; }

        /// <summary> 檔案路徑 </summary>
        public string FilePath { get; set; }

        /// <summary> 檔案名稱 </summary>
        public string FileName { get; set; }

        /// <summary> 原始檔案名稱 </summary>
        public string OrgFileName { get; set; }

        /// <summary> 副檔名 </summary>
        public string FileExtension { get; set; }

        /// <summary> 檔案大小 </summary>
        public int FileSize { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion


        #region Program
        /// <summary> 新增時間 (文字) </summary>
        public string CreateDateText { get { return this.CreateDate.ToString("yyyy-MM-dd"); } }
        #endregion
    }
}
