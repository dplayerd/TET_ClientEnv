using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Models
{
    public class TET_SupplierTradeModel
    {
        public string SubpoenaNo { get; set; }

        public DateTime SubpoenaDate { get; set; }

        public string VenderCode { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
