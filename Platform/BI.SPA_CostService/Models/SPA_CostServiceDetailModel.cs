using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_CostService.Models
{
    public class SPA_CostServiceDetailModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid? ID { get; set; }

        /// <summary> Cost&Service系統辨識碼 </summary>
        public Guid? CSID { get; set; }

        /// <summary> 資料來源 </summary>
        public string Source { get; set; }

        /// <summary> 評鑑與否 </summary>
        public string IsEvaluate { get; set; }

        /// <summary> 評鑑單位 </summary>
        public string BU { get; set; }

        /// <summary> 服務對象 </summary>
        public string ServiceFor { get; set; }

        /// <summary> 受評供應商 </summary>
        public string BelongTo { get; set; }

        /// <summary> PO Source </summary>
        public string POSource { get; set; }

        /// <summary> 評鑑項目 </summary>
        public string AssessmentItem { get; set; }

        /// <summary> 價格競爭力 </summary>
        public string PriceDeflator { get; set; }

        /// <summary> 付款條件 </summary>
        public string PaymentTerm { get; set; }

        /// <summary> 配合度 </summary>
        public string Cooperation { get; set; }

        /// <summary> 優點、滿意、值得鼓勵之處 </summary>
        public string Advantage { get; set; }

        /// <summary> 不滿意、期望改善之處 </summary>
        public string Improved { get; set; }

        /// <summary> 客戶評論與其他補充說明 </summary>
        public string Comment { get; set; }

        /// <summary> 備註 </summary>
        public string Remark { get; set; }

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


        #region 檔案上傳
        /// <summary> 本次上傳的檔案 </summary>
        public List<FileContent> UploadFileList { get; set; } = new List<FileContent>();

        /// <summary> 附件 </summary>
        public List<SPA_CostServiceAttachmentModel> AttachmentList { get; set; }
        #endregion
    }
}
