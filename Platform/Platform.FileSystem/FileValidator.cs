using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Platform.AbstractionClass;

namespace Platform.FileSystem
{
    /// <summary> 檔案檢查器 </summary>
    public class FileValidator
    {
        /// <summary> 1MB </summary>
        private const int _byteToMBSize = 1024 * 1024;

        /// <summary> 檢查檔案是否能上傳 </summary>
        /// <param name="content"> 檔案內容 </param>
        /// <param name="config"> 上傳限制 </param>
        /// <returns></returns>
        public static bool ValidFile(FileContent content, FileValidateConfig config)
        {
            bool valid1 = ValidFileSize(content, config);
            //bool valid2 = ValidFileHeader(content, config);
            bool valid3 = ValidFileMimeType(content, config);

            if (!valid1 || /*!valid2 ||*/ !valid3)
                return false;
            return true;
        }

        /// <summary> 檢查檔案是否超出容量限制 </summary>
        /// <param name="content"> 檔案內容 </param>
        /// <param name="allowSizeMB"> 上傳限制容量 (MB) </param>
        /// <returns></returns>
        private static bool ValidFileSize(FileContent content, int allowSizeMB)
        {
            if (allowSizeMB == -1)
                return true;

            if (allowSizeMB * _byteToMBSize >= content.ContentLength)
                return true;
            else
                return false;
        }

        /// <summary> 檢查檔案是否超出容量限制 </summary>
        /// <param name="content"> 檔案內容 </param>
        /// <param name="config"> 上傳限制 </param>
        /// <returns></returns>
        private static bool ValidFileSize(FileContent content, FileValidateConfig config)
        {
            return ValidFileSize(content, config.AllowSizeMB ?? 0);
        }

        // TODO: 加入檔案簽章檢查
        ///// <summary>
        ///// @param content 
        ///// @param fileHeades 
        ///// @return
        ///// </summary>
        //private static bool ValidFileHeader(FileContent content, string[] fileHeades)
        //{
        //    return false;
        //}

        ///// <summary>
        ///// @param content 
        ///// @param config 
        ///// @return
        ///// </summary>
        //private static bool ValidFileHeader(FileContent content, FileValidateConfig config)
        //{
        //    return ValidFileHeader(content, config.AllowExtesion);
        //}

        /// <summary> 檢查檔案格式是否在允許清單 </summary>
        /// <param name="content"> 檔案內容 </param>
        /// <param name="allowExtensions"> 允許的副檔名 </param>
        /// <returns></returns>
        private static bool ValidFileMimeType(FileContent content, string[] allowExtensions)
        {
            var mimetypes = allowExtensions.Select(obj => MimeMapping.GetMimeMapping(obj).ToLower());
            var fileMime = content.MimeType.ToLower();

            if (!mimetypes.Contains(fileMime))
                return false;
            return true;
        }

        /// <summary> 檢查檔案格式是否在允許清單 </summary>
        /// <param name="content"> 檔案內容 </param>
        /// <param name="config"> 上傳限制 </param>
        /// <returns></returns>
        private static bool ValidFileMimeType(FileContent content, FileValidateConfig config)
        {
            return ValidFileMimeType(content, config.AllowExtensions);
        }
    }
}

