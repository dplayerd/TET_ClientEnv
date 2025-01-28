using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.FileSystem;

namespace Platform.WebSite.Util
{
    public class UploadUtil
    {
        /// <summary> 轉換為資料類別 </summary>
        /// <param name="httpPostedFile"></param>
        /// <returns></returns>
        public static FileContent ConvertToFileContent(HttpPostedFileBase httpPostedFile)
        {
            var postedFile = new FileContent()
            {
                FileName = httpPostedFile.FileName,
                ContentLength = httpPostedFile.ContentLength,
                MimeType = httpPostedFile.ContentType,
                InputStream = httpPostedFile.InputStream,
            };
            return postedFile;
        }

        /// <summary> 轉換為資料類別 </summary>
        /// <param name="httpPostedFile"></param>
        /// <returns></returns>
        public static FileContent ConvertToFileContent(HttpPostedFile httpPostedFile)
        {
            var postedFile = new FileContent()
            {
                FileName = httpPostedFile.FileName,
                ContentLength = httpPostedFile.ContentLength,
                MimeType = httpPostedFile.ContentType,
                InputStream = httpPostedFile.InputStream,
            };
            return postedFile;
        }
    }
}