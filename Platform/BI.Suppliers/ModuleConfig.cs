using BI.Suppliers.Enums;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers
{
    /// <summary> 模組主要設定 </summary>
    public class ModuleConfig
    {
        /// <summary> 個人資訊圖片存檔路徑 </summary>
        public static string FolderPath = "SupplierAttachments";

        /// <summary> 單檔允許容量 </summary>
        public static int AllowSizeMB = 10;

        /// <summary> 檔案總合允許容量 </summary>
        public static int AllowTotalSizeMB = 50;

        /// <summary> 寄信的超連結 </summary>
        public static string EmailRootUrl = "https://FakeDomain/Supplier/";

        /// <summary> 模組名稱 - Supplier </summary>
        public const string ModuleName_Supplier = "Supplier";

        /// <summary> 模組名稱 - SupplierApproval </summary>
        public const string ModuleName_SupplierApproval = "SupplierApproval";

        /// <summary> 模組名稱 - SupplierRevision </summary>
        public const string ModuleName_SupplierRevision = "SupplierRevision";
    }
}
