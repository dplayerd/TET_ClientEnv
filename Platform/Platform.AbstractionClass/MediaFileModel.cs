using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 多媒體檔案 </summary>
    public class MediaFileModel
    {
        public const string DefaultPurpose = "";

        /// <summary> 帳號 </summary>
        public Guid ID { get; set; }

        /// <summary> 模組名稱 </summary>
        public string ModuleName { get; set; }

        /// <summary>模組ID </summary>
        public string ModuleID { get; set; }

        /// <summary> 檔案目的 
        /// <para> 預設值為 Default </para>
        /// </summary>
        public string Purpose { get; set; } = DefaultPurpose;

        /// <summary> 檔案路徑 </summary>
        public string FilePath { get; set; }

        /// <summary> 原始檔案名稱 </summary>
        public string OrgFileName { get; set; }

        /// <summary> 輸出檔案名稱 </summary>
        public string OutputFileName { get; set; }

        /// <summary> 輸出格式名稱 </summary>
        public string MimeType { get; set; }

        /// <summary> 是否需要授權 </summary>
        public bool RequireAuth { get; set; }

        /// <summary> 是否啟用 </summary>
        public bool IsEnable { get; set; }
    }
}
