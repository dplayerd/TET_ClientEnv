using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Utils;
using System;

namespace BI.PaymentSuppliers.Models
{
    /// <summary> 一般付款對象簽核資料 Model </summary>
    public class TET_PaymentSupplierApprovalModel
    {
        #region 資料欄位
        /// <summary> ID </summary>
        public Guid ID { get; set; }

        /// <summary> PSID </summary>
        public Guid PSID { get; set; }

        /// <summary> Type </summary>
        public string Type { get; set; }

        /// <summary> Description </summary>
        public string Description { get; set; }

        /// <summary> Level </summary>
        public string Level { get; set; }

        /// <summary> Approver </summary>
        public string Approver { get; set; }

        /// <summary> Result </summary>
        public string Result { get; set; }

        /// <summary> Comment </summary>
        public string Comment { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary> 是否為預計後續審核步驟 </summary>
        public bool IsSimulated { get; set; }
        #endregion

        #region 預跑欄位
        /// <summary> 預跑關卡顯示名稱 </summary>
        public string PreviewLevel { get; set; }

        /// <summary> 預跑簽核結果顯示文字 </summary>
        public string PreviewResult { get; set; }
        #endregion

        #region 顯示欄位
        /// <summary> 顯示用審核關卡名稱 </summary>
        public string Level_Text
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.PreviewLevel))
                    return this.PreviewLevel;

                var lvl = ApprovalUtils.ParseApprovalLevel(this.Level);

                if (lvl == Enums.ApprovalLevel.Empty)
                    return this.Level;

                return lvl.ToDisplayText();
            }
        }

        /// <summary> 顯示用簽核結果 </summary>
        public string Result_Text
        {
            get
            {
                if (this.IsSimulated && !string.IsNullOrWhiteSpace(this.PreviewResult))
                    return this.PreviewResult;

                return this.Result;
            }
        }

        public Guid ApprovalID { get { return this.ID; } set { this.ID = value; } }

        /// <summary> CreateDate </summary>
        public string CreateDate_Text { get { return this.CreateDate == DateTime.MinValue ? string.Empty : this.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"); } }


        /// <summary> ModifyDate </summary>
        public string ModifyDate_Text { get { return this.ModifyDate == DateTime.MinValue ? string.Empty : this.ModifyDate.ToString("yyyy-MM-dd HH:mm:ss"); } }
        #endregion
    }
}
