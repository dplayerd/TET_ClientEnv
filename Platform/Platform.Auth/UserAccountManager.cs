using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Platform.ORM;
using Platform.AbstractionClass;
using Platform.LogService;
using Platform.Infra;
using Platform.Security;
using Platform.Auth.Models;
using Platform.Auth.InternalUtility;
using Platform.FileSystem;

namespace Platform.Auth
{
    public class UserAccountManager
    {
        private Logger _logger = new Logger();

        ///// <summary> 取得使用者清單 </summary>
        ///// <param name="pager"></param>
        ///// <returns></returns>
        //public List<UserAccountModel> GetUserList(Pager pager)
        //{
        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var query =
        //                from item in context.SystemUsers
        //                where item.IsDeleted == false
        //                orderby item.CreateDate descending
        //                select new UserAccountModel()
        //                {
        //                    EmpID = item.Account,
        //                    EMail = item.Email,
        //                    ID = item.ID,
        //                    FirstNameCH = item.FirstName,
        //                    LastNameCH = item.LastName,

        //                    IsEnable = item.IsEnable,
        //                };

        //            var list = query.ProcessPager(pager).ToList();
        //            return list;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError(ex);
        //        return default;
        //    }
        //}

        ///// <summary> 使用帳號查詢使用者
        ///// <para> 請勿將此回傳值直接回傳 Client 端 (有資安疑慮) </para>
        ///// </summary>
        ///// <param name="account"></param>
        ///// <returns></returns>
        //internal SystemUser GetUser(string account)
        //{
        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var query =
        //                from item in context.SystemUsers
        //                where item.IsDeleted == false && item.Account == account
        //                select item;

        //            var result = query.FirstOrDefault();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError(ex);
        //        return default;
        //    }
        //}

        ///// <summary> 取得使用者 </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public UserAccountModel GetUser(Guid userID)
        //{
        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var query =
        //                from item in context.SystemUsers
        //                where item.IsDeleted == false && item.ID == userID
        //                select new UserAccountModel()
        //                {
        //                    EmpID = item.Account,
        //                    EMail = item.Email,
        //                    ID = item.ID,
        //                    FirstNameCH = item.FirstName,
        //                    LastNameCH = item.LastName,

        //                    IsEnable = item.IsEnable,
        //                };

        //            var result = query.FirstOrDefault();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError(ex);
        //        return default;
        //    }
        //}

        ///// <summary> 建立使用者 </summary>
        ///// <param name="user"></param>
        ///// <param name="password"></param>
        ///// <param name="currentUserID"></param>
        //public void CreateUser(UserAccountModel user, string password, Guid currentUserID)
        //{
        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            SystemUser dbUser = new SystemUser()
        //            {
        //                ID = Guid.NewGuid(),
        //                FirstName = user.FirstNameCH,
        //                LastName = user.LastNameCH,
        //                Account = user.EmpID,
        //                Email = user.EMail,
        //                Title = user.Title,

        //                IsDeleted = false,
        //                CreateUser = currentUserID,
        //                CreateDate = DateTime.Now
        //            };

        //            byte[] hashKey = dbUser.ID.ToByteArray();
        //            dbUser.HashKey = ByteUtility.BytesToBase64String(hashKey);
        //            dbUser.Password = PWDUtility.CalculatePWD(password, dbUser);

        //            context.SystemUsers.Add(dbUser);
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError(ex);
        //        throw;
        //    }
        //}

        ///// <summary> 修改使用者 </summary>
        ///// <param name="user"></param>
        ///// <param name="currentUserID"></param>
        //public void ModifyUser(UserAccountModel user, Guid currentUserID)
        //{
        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var dbUser = context.SystemUsers.Where(obj => obj.ID == user.ID).FirstOrDefault();

        //            if (dbUser != null)
        //            {
        //                dbUser.FirstName = user.FirstNameCH;
        //                dbUser.LastName = user.LastNameCH;
        //                dbUser.Email = user.EMail;
        //                dbUser.Title = user.Title;

        //                dbUser.ModifyUser = currentUserID;
        //                dbUser.ModifyDate = DateTime.Now;
        //            }

        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError(ex);
        //        throw;
        //    }
        //}

        ///// <summary> 刪除使用者 </summary>
        ///// <param name="user"></param>
        ///// <param name="currentUserID"></param>
        //public void DeleteUser(UserAccountModel user, Guid currentUserID)
        //{
        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var dbUser = context.SystemUsers.Where(obj => obj.ID == user.ID).FirstOrDefault();

        //            if (dbUser != null)
        //            {
        //                dbUser.IsDeleted = true;
        //                dbUser.DeleteUser = currentUserID;
        //                dbUser.DeleteDate = DateTime.Now;
        //            }

        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError(ex);
        //        throw;
        //    }
        //}

        ///// <summary> 修改密碼 </summary>
        ///// <param name="user"></param>
        ///// <param name="currentUserID"></param>
        ///// <param name="newPassword"></param>
        //public void ChangePassword(UserAccountModel user, Guid currentUserID, string newPassword, out string msg)
        //{
        //    msg = null;
        //    if (!CheckPassword(newPassword))
        //    {
        //        msg = "密碼需介於8~16字之間，且至少須包含1個大寫英文數字，1個小寫英文數字，1個特殊符號組成及1位數字";
        //        return;
        //    }

        //    try
        //    {
        //        using (PlatformContextModel context = new PlatformContextModel())
        //        {
        //            var dbUser = context.SystemUsers.Where(obj => obj.ID == user.ID).FirstOrDefault();

        //            if (dbUser != null)
        //            {
        //                byte[] hashKey = dbUser.ID.ToByteArray();
        //                dbUser.HashKey = ByteUtility.BytesToBase64String(hashKey);
        //                dbUser.Password = PWDUtility.CalculatePWD(newPassword, dbUser);

        //                dbUser.ModifyUser = currentUserID;
        //                dbUser.ModifyDate = DateTime.Now;
        //            }

        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteError(ex);
        //        throw;
        //    }
        //}

        ///// <summary> 變更使用者圖示 </summary>
        ///// <param name="userID"> 要更換圖示的使用者 </param>
        ///// <param name="content">圖片資訊</param>
        ///// <param name="currentUserID">目前登入者</param>
        //public void ChangeProfileImage(Guid userID, FileContent content, Guid currentUserID)
        //{
        //    DateTime cDate = DateTime.Now;


        //    // 取得使用者
        //    var user = this.GetUser(userID);
        //    if (user == null)
        //        throw new NullReferenceException($"User doesn't exist: [{userID}]");


        //    using (PlatformContextModel context = new PlatformContextModel())
        //    {
        //        var fileManager = new MediaFileManager();
                
        //        // 取得使用者舊頭像，如果舊頭像存在，刪除之
        //        var oldFile = fileManager.GetAdminMediaFile(context, ModuleConfig.ModuleName, userID.ToString());
        //        if (oldFile != null)
        //            fileManager.DeleteDataAndFile(context, oldFile.ID, userID, cDate);

        //        // 儲存資料庫
        //        MediaFileModel mediaFileModel = new MediaFileModel()
        //        {
        //            ModuleName = ModuleConfig.ModuleName,
        //            ModuleID = userID.ToString(),
        //            MimeType = content.MimeType,
        //            FilePath = ModuleConfig.ProfileFolderPath,
        //            RequireAuth = false,
        //            OutputFileName = content.FileName,
        //            OrgFileName = content.FileName,
        //        };

        //        // 存檔
        //        fileManager.UploadAndCreate(context, mediaFileModel, content, currentUserID, cDate);
        //        context.SaveChanges();
        //    }
        //}

        ///// <summary>
        ///// 密碼原則
        ///// </summary>
        ///// <param name="pwd"></param>
        ///// <returns></returns>
        //private static bool CheckPassword(string pwd)
        //{
        //    Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W).{8,16}$");
        //    if (!regex.IsMatch(pwd))
        //        return false;
        //    return true;
        //}
    }
}
