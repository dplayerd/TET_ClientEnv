using System;
using System.Collections.Generic;
using System.Linq;
using BI.Suppliers.Models;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;

namespace BI.Suppliers.Validators
{
    public class SupplierAttachmentValidator
    {
        /// <summary> 1MB </summary>
        private const int _byteToMBSize = 1024 * 1024;

        // 錯誤訊息
        private const string _alarmFileNameText = " 檔名不允許重覆";
        private const string _alarmSizeText = " {0} 超出容量限制，單檔最大容量為 {1} MB";
        private const string _alarmTotalSizeText = "超出總容量限制，全部檔案最大容量為 {0} MB";


        /// <summary> 驗證新增資料 </summary>
        /// <param name="modelList"> 輸入資料 </param>
        /// <param name="uploadList"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(List<TET_SupplierAttachmentModel> modelList, List<FileContent> uploadList, out List<string> msgList)
        {
            msgList = new List<string>();
            long totalSize = 0;

            // 驗證檔名
            if (uploadList.Select(obj => obj.FileName).Intersect(modelList.Select(obj => obj.OrgFileName)).Any())
                msgList.Add(_alarmFileNameText);

            // 驗證單檔容量
            foreach (var item in modelList)
            {
                if (!ValidFileSize(item.FileSize, ModuleConfig.AllowSizeMB))
                    msgList.Add(string.Format(_alarmSizeText, item.OrgFileName, ModuleConfig.AllowSizeMB));

                totalSize += item.FileSize;
            }

            // 驗證上傳的每個檔案容量
            foreach (var item in uploadList)
            {
                if (!ValidFileSize(item, ModuleConfig.AllowSizeMB))
                    msgList.Add(string.Format(_alarmSizeText, item.FileName, ModuleConfig.AllowSizeMB));
                totalSize += item.ContentLength;
            }

            // 驗證總容量
            if (!ValidFileSize(totalSize, ModuleConfig.AllowTotalSizeMB))
                msgList.Add(string.Format(_alarmTotalSizeText, ModuleConfig.AllowTotalSizeMB));


            if (msgList.Count > 0)
                return false;
            else
                return true;
        }


        /// <summary> 檢查檔案是否超出容量限制 </summary>
        /// <param name="fileSize"> 檔案容量 </param>
        /// <param name="allowSizeMB"> 上傳限制容量 (MB) </param>
        /// <returns></returns>
        private static bool ValidFileSize(long fileSize, int allowSizeMB)
        {
            if (allowSizeMB == -1)
                return true;

            if (allowSizeMB * _byteToMBSize >= fileSize)
                return true;
            else
                return false;
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

    }
}
