using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 檔案上傳驗證設定 </summary>
    public class FileValidateConfig
    {
        public FileValidateConfig()
        { }

        public FileValidateConfig(string moduleName, string purpose, int? allowSizeMB, string[] allowExtensions)
        {
            this.ModuleName = moduleName;
            this.Purpose = purpose;
            this.AllowSizeMB = allowSizeMB;
            this.AllowExtensions = allowExtensions;
        }

        /// <summary> 模組名稱 </summary>
        public string ModuleName { get; set; }

        /// <summary> 檔案用途 </summary>
        public string Purpose { get; set; }

        /// <summary> 允許上傳的容量
        /// <para> (-1 代表不限制) (null 代表繼承上層設定) </para>
        /// </summary>
        public int? AllowSizeMB { get; set; } = null;

        /// <summary> 允許上傳的副檔名 </summary>
        public string[] AllowExtensions { get; set; }
    }
}
