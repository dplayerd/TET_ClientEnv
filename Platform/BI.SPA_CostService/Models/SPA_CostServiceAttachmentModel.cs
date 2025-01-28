using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_CostService.Models
{
    public class SPA_CostServiceAttachmentModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> CSDetailID </summary>
        public Guid CSDetailID { get; set; }

        /// <summary> FilePath </summary>
        public string FilePath { get; set; }

        /// <summary> FileName </summary>
        public string FileName { get; set; }

        /// <summary> OrgFileName </summary>
        public string OrgFileName { get; set; }

        /// <summary> FileExtension </summary>
        public string FileExtension { get; set; }

        /// <summary> FileSize </summary>
        public int FileSize { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }
        #endregion
    }
}
