using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Models
{
    public class PaymentSupplierListModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> 中文名稱 </summary>
        public string CName { get; set; }

        /// <summary> 英文名稱 </summary>
        public string EName { get; set; }

        /// <summary> 供應商代碼 </summary>
        public string VenderCode { get; set; }

        /// <summary> 統一編號 </summary>
        public string TaxNo { get; set; }

        /// <summary> 身分證字號 </summary>
        public string IdNo { get; set; }


        /// <summary> 版本編號 </summary>
        public int Version { get; set; }

        /// <summary> 是否為最新版本 </summary>
        public string IsLastVersion { get; set; }

        /// <summary> 審核狀態 </summary>
        public string ApproveStatus { get; set; }

        /// <summary> 申請人 </summary>
        public string CreateUser { get; set; }
        #endregion

        #region 外部欄位
        /// <summary> 審核關卡 </summary>
        public string Level { get; set; }

        /// <summary> 顯示用審核關卡名稱 </summary>
        public string Level_Text
        {
            get
            {
                var lvl = ApprovalUtils.ParseApprovalLevel(this.Level);
                return lvl.ToDisplayText();
            }
        }
        #endregion
    }
}
