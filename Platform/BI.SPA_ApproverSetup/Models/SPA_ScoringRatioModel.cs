using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Models
{
    public class SPA_ScoringRatioModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 評鑑項目系統辨識碼 </summary>
        public Guid ServiceItemID { get; set; }

        /// <summary> PO Source </summary>
        public string POSource { get; set; }

        /// <summary> 施工正確性比例或作業正確性比例 </summary>
        public decimal TRatio1 { get; set; }

        /// <summary> 施工技術水平比例 </summary>
        public decimal TRatio2 { get; set; }

        /// <summary> 人員穩定度比例 </summary>
        public decimal DRatio1 { get; set; }

        /// <summary> 準時完工交付比例或人員備齊貢獻度比例 </summary>
        public decimal DRatio2 { get; set; }

        /// <summary> 守規性比例 </summary>
        public decimal QRatio1 { get; set; }

        /// <summary> 自訓能力比例 </summary>
        public decimal QRatio2 { get; set; }

        /// <summary> 價格競爭力比例 </summary>
        public decimal CRatio1 { get; set; }

        /// <summary> 付款條件比例 </summary>
        public decimal CRatio2 { get; set; }

        /// <summary> 客戶抱怨比例 </summary>
        public decimal SRatio1 { get; set; }

        /// <summary> 配合度比例 </summary>
        public decimal SRatio2 { get; set; }

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
        public string ServiceItem { get; set; }
        #endregion
    }
}
