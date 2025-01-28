using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Models
{
    /// <summary> 一般付款對象搜尋過濾條件 </summary>
    public interface ISupplierListQueryInput
    {
        /// <summary> 中文、英文名稱、供應商代碼 </summary>
        string caption { get; set; }

        /// <summary> 統一編號、身分證字號 </summary>
        string taxNo { get; set; }
    }
}
