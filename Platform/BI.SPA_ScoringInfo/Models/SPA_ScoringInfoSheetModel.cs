using System;

namespace BI.SPA_ScoringInfo.Models
{
    /// <summary> SPA 評鑑計分資料頁籤設定 </summary>
    public class SPA_ScoringInfoSheetModel
    {
        #region 原生欄位
        /// <summary> 評鑑項目系統辨識碼 </summary>
        public Guid ServiceItemID { get; set; }

        /// <summary> PO Source </summary>
        public string POSource { get; set; }


        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion


        #region 原生欄位 - Sheet1
        /// <summary> 人工盤點頁籤是否顯示 </summary>
        public bool IsSheet1Show { get; set; }

        /// <summary> 人工盤點頁籤本社/協力廠商欄位是否必填 </summary>
        public bool IsSheet1TypeFill { get; set; }

        /// <summary> 人工盤點頁籤供應商名稱欄位是否必填 </summary>
        public bool IsSheet1SupplierFill { get; set; }

        /// <summary> 人工盤點頁籤資料來源欄位是否必填 </summary>
        public bool IsSheet1SourceFill { get; set; }

        /// <summary> 人工盤點頁籤員工姓名欄位是否必填 </summary>
        public bool IsSheet1EmpNameFill { get; set; }

        /// <summary> 人工盤點頁籤主要負責作業欄位是否必填 </summary>
        public bool IsSheet1MajorJobFill { get; set; }

        /// <summary> 人工盤點頁籤能否獨立作業欄位是否必填 </summary>
        public bool IsSheet1IsIndependentFill { get; set; }

        /// <summary> 人工盤點頁籤 Skill Level 欄位是否必填 </summary>
        public bool IsSheet1SkillLevelFill { get; set; }

        /// <summary> 人工盤點頁籤員工狀態欄位是否必填 </summary>
        public bool IsSheet1EmpStatusFill { get; set; }

        /// <summary> 人工盤點頁籤派工至 TEL 的年資（年）欄位是否必填 </summary>
        public bool IsSheet1TELSeniorityYFill { get; set; }

        /// <summary> 人工盤點頁籤派工至 TEL 的年資（月）欄位是否必填 </summary>
        public bool IsSheet1TELSeniorityMFill { get; set; }

        /// <summary> 人工盤點頁籤備註欄位是否必填 </summary>
        public bool IsSheet1RemarkFill { get; set; }
        #endregion


        #region 原生欄位 - Sheet2
        /// <summary> 施工達交狀況盤點頁籤是否顯示 </summary>
        public bool IsSheet2Show { get; set; }

        /// <summary> 施工達交狀況盤點頁籤服務對象欄位是否必填 </summary>
        public bool IsSheet2ServiceForFill { get; set; }

        /// <summary> 施工達交狀況盤點頁籤作業項目欄位是否必填 </summary>
        public bool IsSheet2WorkItemFill { get; set; }

        /// <summary> 施工達交狀況盤點頁籤承攬機台名稱欄位是否必填 </summary>
        public bool IsSheet2MachineNameFill { get; set; }

        /// <summary> 施工達交狀況盤點頁籤機台 Serial No. 欄位是否必填 </summary>
        public bool IsSheet2MachineNoFill { get; set; }

        /// <summary> 施工達交狀況盤點頁籤是否準時交付欄位是否必填 </summary>
        public bool IsSheet2OnTimeFill { get; set; }

        /// <summary> 施工達交狀況盤點頁籤備註欄位是否必填 </summary>
        public bool IsSheet2RemarkFill { get; set; }
        #endregion


        #region 原生欄位 - Sheet3
        /// <summary> 施工正確性頁籤是否顯示 </summary>
        public bool IsSheet3Show { get; set; }

        /// <summary> 施工正確性頁籤出工人數欄位是否必填 </summary>
        public bool IsSheet3WorkerCountFill { get; set; }

        /// <summary> 施工正確性頁籤時間欄位是否必填 </summary>
        public bool IsSheet3DateFill { get; set; }

        /// <summary> 施工正確性頁籤地點欄位是否必填 </summary>
        public bool IsSheet3LocationFill { get; set; }

        /// <summary> 施工正確性頁籤 TEL 財損欄位是否必填 </summary>
        public bool IsSheet3TELLossFill { get; set; }

        /// <summary> 施工正確性頁籤客戶財損欄位是否必填 </summary>
        public bool IsSheet3CustomerLossFill { get; set; }

        /// <summary> 施工正確性頁籤人身事故欄位是否必填 </summary>
        public bool IsSheet3AccidentFill { get; set; }

        /// <summary> 施工正確性頁籤事件說明欄位是否必填 </summary>
        public bool IsSheet3DescriptionFill { get; set; }
        #endregion


        #region 原生欄位 - Sheet4
        /// <summary> 作業正確性 &amp; 人員備齊貢獻度頁籤是否顯示 </summary>
        public bool IsSheet4Show { get; set; }

        /// <summary> 作業正確性 &amp; 人員備齊貢獻度頁籤作業正確性欄位是否必填 </summary>
        public bool IsSheet4CorrectnessFill { get; set; }

        /// <summary> 作業正確性 &amp; 人員備齊貢獻度頁籤人員備齊貢獻度欄位是否必填 </summary>
        public bool IsSheet4ContributionFill { get; set; }
        #endregion


        #region 原生欄位 - Sheet5
        /// <summary> 自訓能力頁籤是否顯示 </summary>
        public bool IsSheet5Show { get; set; }

        /// <summary> 自訓能力頁籤供應商自訓程度欄位是否必填 </summary>
        public bool IsSheet5SelfTrainingFill { get; set; }

        /// <summary> 自訓能力頁籤備註欄位是否必填 </summary>
        public bool IsSheet5SelfTrainingRemarkFill { get; set; }
        #endregion


        #region 原生欄位 - Sheet6
        /// <summary> 服務頁籤是否顯示 </summary>
        public bool IsSheet6Show { get; set; }

        /// <summary> 服務頁籤配合度欄位是否必填 </summary>
        public bool IsSheet6CooperationFill { get; set; }

        /// <summary> 服務頁籤時間欄位是否必填 </summary>
        public bool IsSheet6DateFill { get; set; }

        /// <summary> 服務頁籤地點欄位是否必填 </summary>
        public bool IsSheet6LocationFill { get; set; }

        /// <summary> 服務頁籤造成財損欄位是否必填 </summary>
        public bool IsSheet6IsDamageFill { get; set; }

        /// <summary> 服務頁籤事件說明欄位是否必填 </summary>
        public bool IsSheet6DescriptionFill { get; set; }
        #endregion


        #region 原生欄位 - Sheet7
        /// <summary> 附件頁籤是否顯示 </summary>
        public bool IsSheet7Show { get; set; }
        #endregion

        #region Program
        public string ServiceItem { get; set; }
        #endregion
    }
}
