using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Models
{
    public class SPA_ViolationDetailModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid? ID { get; set; }

        /// <summary> 違規紀錄系統辨識碼 </summary>
        public Guid? ViolationID { get; set; }

        /// <summary> 日期 </summary>
        public DateTime Date { get; set; }

        /// <summary> 供應商名稱 </summary>
        public string BelongTo { get; set; }

        /// <summary> 評鑑單位 </summary>
        public string BU { get; set; }

        /// <summary> 評鑑項目 </summary>
        public string AssessmentItem { get; set; }

        /// <summary> 中分類 </summary>
        public string MiddleCategory { get; set; }

        /// <summary> 小分類 </summary>
        public string SmallCategory { get; set; }

        /// <summary> 客戶名稱 </summary>
        public string CustomerName { get; set; }

        /// <summary> 客戶廠別 </summary>
        public string CustomerPlant { get; set; }

        /// <summary> 客戶細分 </summary>
        public string CustomerDetail { get; set; }

        /// <summary> 違規事項說明 </summary>
        public string Description { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion

        #region 程式用
        /// <summary> 用來讀取 Client 端的序號 </summary>
        public int Index { get; set; } = 0;
        #endregion

        #region Program
        /// <summary> 日期 </summary>
        public string Date_Text { get { return this.Date.ToString("yyyy-MM-dd"); } }
        #endregion


        #region 檔案上傳
        /// <summary> 本次上傳的檔案 </summary>
        public List<FileContent> UploadFileList { get; set; } = new List<FileContent>();

        /// <summary> 附件 </summary>
        public List<SPA_ViolationAttachmentModel> AttachmentList { get; set; }
        #endregion
    }
}
