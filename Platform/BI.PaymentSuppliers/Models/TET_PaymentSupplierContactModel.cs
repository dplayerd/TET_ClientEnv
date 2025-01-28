using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Models
{
    public class TET_PaymentSupplierContactModel
    {
        /// <summary> ID </summary>
        public Guid ID { get; set; }

        /// <summary> PSID </summary>
        public Guid PSID { get; set; }

        /// <summary> ContactName </summary>
        public string ContactName { get; set; }

        /// <summary> ContactTitle </summary>
        public string ContactTitle { get; set; }

        /// <summary> ContactTel </summary>
        public string ContactTel { get; set; }

        /// <summary> ContactEmail </summary>
        public string ContactEmail { get; set; }

        /// <summary> ContactRemark </summary>
        public string ContactRemark { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }
    }
}
