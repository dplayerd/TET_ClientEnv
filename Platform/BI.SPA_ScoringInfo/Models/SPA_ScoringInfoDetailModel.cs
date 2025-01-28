using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models
{
    public class SPA_ScoringInfoDetailModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> CSID </summary>
        public Guid? CSID { get; set; }

        /// <summary> Source </summary>
        public string Source { get; set; }

        /// <summary> IsEvaluate </summary>
        public string IsEvaluate { get; set; }

        /// <summary> BU </summary>
        public string BU { get; set; }

        /// <summary> ServiceFor </summary>
        public string ServiceFor { get; set; }

        /// <summary> BelongTo </summary>
        public string BelongTo { get; set; }

        /// <summary> POSource </summary>
        public string POSource { get; set; }

        /// <summary> AssessmentItem </summary>
        public string AssessmentItem { get; set; }

        /// <summary> PriceDeflator </summary>
        public string PriceDeflator { get; set; }

        /// <summary> PaymentTerm </summary>
        public string PaymentTerm { get; set; }

        /// <summary> Cooperation </summary>
        public string Cooperation { get; set; }

        /// <summary> Advantage </summary>
        public string Advantage { get; set; }

        /// <summary> Improved </summary>
        public string Improved { get; set; }

        /// <summary> Comment </summary>
        public string Comment { get; set; }

        /// <summary> Remark </summary>
        public string Remark { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
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
        public List<SPA_ScoringInfoAttachmentModel> AttachmentList { get; set; }
        #endregion
    }
}
