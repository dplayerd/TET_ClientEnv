using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Models
{
    /// <summary> 供應商搜尋過濾條件 </summary>
    public interface ISupplierListQueryInput
    {
        /// <summary> 中文、英文名稱、供應商代碼 </summary>
        string caption { get; set; }

        /// <summary> 統一編號 </summary>
        string taxNo { get; set; }

        /// <summary> 歸屬公司 </summary>
        string[] belongTo { get; set; }

        /// <summary> 廠商類別 </summary>
        string[] supplierCategory { get; set; }

        /// <summary> 交易主類別 </summary>
        string[] businessCategory { get; set; }

        /// <summary> 交易子類別 </summary>
        string[] businessAttribute { get; set; }

        /// <summary> 主要產品/服務項目 </summary>
        string mainProduct { get; set; }

        /// <summary> 關鍵字 </summary>
        string searchKey { get; set; }

        /// <summary> 主要供應商 </summary>
        string keySupplier { get; set; }

        /// <summary> STQA 憑證 </summary>
        string stqaCertified { get; set; }

        /// <summary> 採購擔當 </summary>
        string[] buyer { get; set; }
    }
}
