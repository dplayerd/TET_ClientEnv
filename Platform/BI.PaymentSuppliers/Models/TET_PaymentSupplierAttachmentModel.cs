using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Models
{
    public class TET_PaymentSupplierAttachmentModel
    {
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> PSID </summary>
        public Guid PSID { get; set; }

        /// <summary> FilePath </summary>
        public string FilePath { get; set; }

        /// <summary> FileName </summary>
        public string FileName { get; set; }

        /// <summary> OrgFileName </summary>
        public string OrgFileName { get; set; }

        /// <summary> FileExtension </summary>
        public string FileExtension { get; set; }

        /// <summary> FileSize </summary>
        public int FileSize{ get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }


        #region 計算欄位
        public Guid? SupplierAttachmentID { get { return this.ID; } set { this.ID = value; } }
        public string SupplierAttachmentFileName { get { return this.OrgFileName; } }
        public string SupplierAttachmentCreateDate { get { return this.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"); } }
        #endregion
    }
}
