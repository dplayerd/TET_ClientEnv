using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 上傳的檔案 </summary>
    public class FileContent
    {
        /// <summary> 檔案容量 (Byte) </summary>
        public int ContentLength { get; set; }

        /// <summary> MimeType </summary>
        public string MimeType { get; set; }

        /// <summary> 檔名 </summary>
        public string FileName { get; set; }

        /// <summary> 原始檔案內容 </summary>
        public Stream InputStream { get; set; }
    }
}
