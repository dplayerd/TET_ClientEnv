using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BI.SPA_ApproverSetup.Models
{
    /// <summary> SPA評鑑審核者設定 </summary>
    public class TET_SPA_ApproverSetupModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid ID { get; set; }

        /// <summary> 評鑑項目系統辨識碼 </summary>
        public Guid ServiceItemID { get; set; }

        /// <summary> 評鑑單位系統辨識碼 </summary>
        public Guid BUID { get; set; }

        /// <summary> 計分資料填寫者 </summary>
        public string InfoFill { get; set; }

        /// <summary> 計分資料確認者 </summary>
        public string InfoConfirm { get; set; }

        /// <summary> 第一關審核者 </summary>
        public string Lv1Apprvoer { get; set; }

        /// <summary> 第二關審核者 </summary>
        public string Lv2Apprvoer { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion


        #region Other Table
        public string ServiceItemText { get; set; }
        public string BUText { get; set; }

        /// <summary> InfoFill </summary>
        public string[] InfoFills
        {
            get
            {
                if (this.InfoFill == null)
                    this.InfoFill = "[]";
                return JsonConvert.DeserializeObject<string[]>(this.InfoFill);
            }
            set
            {
                this.InfoFill = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary> 含完整工號資訊及個人資訊 </summary>
        public List<string> InfoFillUserInfos { get; set; } = new List<string>();
        #endregion
    }
}
