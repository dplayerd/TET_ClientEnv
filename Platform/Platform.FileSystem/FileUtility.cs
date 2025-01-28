using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;
using Platform.LogService;
using Platform.Security;

namespace Platform.FileSystem
{
    public class FileUtility
    {
        #region "Private Methods"
        /// <summary> 以時間為主，建立新檔名 (保留副檔名) </summary>
        /// <param name="orgFileName"></param>
        /// <returns></returns>
        private static string GenerateNewFileName(string orgFileName)
        {
            string fileExt = Path.GetExtension(orgFileName);
            string randomText = RandomUtility.Next(999).ToString("000");
            string nowText = DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFFFF");
            string newFileName = nowText + randomText + fileExt;
            return newFileName;
        }
        #endregion

        /// <summary> 儲存檔案至指定路徑 </summary>
        /// <param name="file"> 原始檔案 </param>
        /// <param name="saveFolder"> 要存檔的資料夾 </param>
        /// <param name="saveAsName"> 要存檔的名稱，如果留空會自動重新命名 </param>
        /// <returns></returns>
        public static string Upload(FileContent file, string saveFolder, string saveAsName = "")
        {
            string fileFullName =
                (string.IsNullOrWhiteSpace(saveAsName))
                    ? GenerateNewFileName(file.FileName)
                    : saveAsName;

            string filePath = Path.Combine(saveFolder, fileFullName);

            try
            {
                FileUtility.CreateFolder(saveFolder);

                using (var fileStream = File.Create(filePath, file.ContentLength))
                {
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    file.InputStream.CopyTo(fileStream);

                    fileStream.Flush();
                }
            }
            catch (Exception ex)
            {
                new Logger().WriteError(ex);
                throw;
            }

            return fileFullName;
        }

        /// <summary> 刪除檔案 </summary>
        /// <param name="filePath"> 檔名 </param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary> 移動檔案至指定資料夾 </summary>
        /// <param name="sourceFilePath"> 來源檔案路徑 </param>
        /// <param name="destFolderPath"> 目標資料夾路徑 </param>
        public static void MoveFile(string sourceFilePath, string destFolderPath)
        {
            if (!File.Exists(sourceFilePath) ||
                !Directory.Exists(destFolderPath))
                throw new IOException($" File doesn't exist: [{sourceFilePath}, {destFolderPath}] ");

            string fileName = Path.GetFileName(sourceFilePath);
            string newFilePath = Path.Combine(destFolderPath, fileName);

            if (File.Exists(newFilePath))
                throw new IOException($" File exists: [{newFilePath}] ");

            File.Move(sourceFilePath, newFilePath);
        }

        /// <summary> 複製檔案至指定資料夾 </summary>
        /// <param name="sourceFilePath"> 來源檔案路徑 </param>
        /// <param name="destFolderPath"> 目標資料夾路徑 </param>
        public static void CopyFile(string sourceFilePath, string destFolderPath)
        {
            if (!File.Exists(sourceFilePath) ||
                !Directory.Exists(destFolderPath))
                throw new IOException($" File doesn't exist: [{sourceFilePath}, {destFolderPath}] ");

            string fileName = Path.GetFileName(sourceFilePath);
            string newFilePath = Path.Combine(destFolderPath, fileName);

            if (File.Exists(newFilePath))
                throw new IOException($" File exists: [{newFilePath}] ");

            File.Copy(sourceFilePath, newFilePath);
        }

        /// <summary> 複製檔案至指定資料夾 </summary>
        /// <param name="sourceFilePath"> 來源檔案路徑 </param>
        /// <param name="destFolderPath"> 目標資料夾路徑 </param>
        public static string CopyAndRenameFile(string sourceFilePath, string destFolderPath)
        {
            if (!File.Exists(sourceFilePath) ||
                !Directory.Exists(destFolderPath))
                throw new IOException($" File doesn't exist: [{sourceFilePath}, {destFolderPath}] ");

            string fileName = Path.GetFileName(sourceFilePath);
            string newFileName = GenerateNewFileName(fileName);
            string newFilePath = Path.Combine(destFolderPath, newFileName);

            if (File.Exists(newFilePath))
                throw new IOException($" File exists: [{newFilePath}] ");

            File.Copy(sourceFilePath, newFilePath);
            return newFileName;
        }

        /// <summary> 取得檔案 Stream </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        public FileStream GetStream(string filePath)
        {
            if (!File.Exists(filePath))
                throw new IOException($" File doesn't exist: [{filePath}] ");

            using (FileStream fs = File.OpenRead(filePath))
            {
                return fs;
            }
        }

        /// <summary> 建立資料夾 </summary>
        /// <param name="folderPath"> 資料夾路徑 (必須以 '/' 或 '\' 符號結尾) </param>
        public static void CreateFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
                return;

            Directory.CreateDirectory(folderPath);
        }

        /// <summary> 刪除資料夾及裡面所有檔案 (包含子資料夾) </summary>
        /// <param name="folderPath"> 資料夾路徑 (必須以 '/' 或 '\' 符號結尾) </param>
        public static void DeleteFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;

            DirectoryInfo folder = new DirectoryInfo(folderPath);
            folder.Delete(true);
        }

        /// <summary> 移動資料夾 </summary>
        /// <param name="sourceFolderPath"> 來源資料夾路徑 (必須以 '/' 或 '\' 符號結尾) </param>
        /// <param name="destFolderPath"> 目標資料夾路徑 (必須以 '/' 或 '\' 符號結尾) </param>
        public static void MoveFolder(string sourceFolderPath, string destFolderPath)
        {
            if (!Directory.Exists(sourceFolderPath) ||
                !Directory.Exists(destFolderPath))
                throw new IOException($" Folder doesn't exist: [{sourceFolderPath}, {destFolderPath}] ");

            Directory.Move(sourceFolderPath, destFolderPath);
        }
    }
}
