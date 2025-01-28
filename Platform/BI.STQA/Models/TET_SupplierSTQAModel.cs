using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.STQA.Models
{
    public class TET_SupplierSTQAModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> BelongTo </summary>
        public string BelongTo { get; set; }

        /// <summary> Purpose </summary>
        public string Purpose { get; set; }

        /// <summary> BusinessTerm </summary>
        public string BusinessTerm { get; set; }

        /// <summary> Date </summary>
        public DateTime? Date { get; set; }

        /// <summary> Type </summary>
        public string Type { get; set; }

        /// <summary> UnitALevel </summary>
        public string UnitALevel { get; set; }

        /// <summary> UnitCLevel </summary>
        public string UnitCLevel { get; set; }

        /// <summary> UnitDLevel </summary>
        public string UnitDLevel { get; set; }

        /// <summary> Comment </summary>
        public string STQAComment { get; set; }

        /// <summary> ApproveStatus </summary>
        public string ApproveStatus { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }
        #endregion

        #region 外部資料表
        /// <summary> 簽核歷程 </summary>
        public List<TET_SupplierSTQAApprovalModel> ApprovalList { get; set; } = new List<TET_SupplierSTQAApprovalModel>();
        #endregion

        #region 其它欄位
        public string Date_Text { get { return this.Date?.ToString("yyyy/MM/dd"); } }
        #endregion
    }
}
