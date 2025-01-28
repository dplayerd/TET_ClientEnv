using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.AllApproval.Models
{
    /// <summary> 所有簽核共用的 Model </summary>
    public class ApprovalModel
    {
        /// <summary> ID </summary>
        public Guid ID { get; set; }

        /// <summary> ParentID </summary>
        public Guid ParentID { get; set; }

        /// <summary> ParentType </summary>
        public string ParentType { get; set; }

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

        #region 其它欄位
        /// <summary> CreateDate </summary>
        public string CreateDate_Text { get { return this.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"); } }

        /// <summary> ModifyDate </summary>
        public string ModifyDate_Text { get { return this.ModifyDate.ToString("yyyy-MM-dd HH:mm:ss"); } }

        public string Level_Text { get; set; }
        #endregion
    }
}
