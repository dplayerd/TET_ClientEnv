using Platform.AbstractionClass;
using Platform.Auth.Models;
using Platform.LogService;
using Platform.ORM;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Auth
{
    public class UserManager
    {
        private Logger _logger = new Logger();

        #region Read
        /// <summary>
        /// 取得 使用者 清單
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<UserModel> GetUserList(Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.Users
                    where
                        item.IsEnabled == "Y"
                    orderby item.UserID
                    select
                    new UserModel()
                    {
                        UserID = item.UserID,
                        EmpID = item.EmpID,
                        FirstNameEN = item.FirstNameEN,
                        LastNameEN = item.LastNameEN,
                        FirstNameCH = item.FirstNameCH,
                        LastNameCH = item.LastNameCH,
                        UnitCode = item.UnitCode,
                        UnitName = item.UnitName,
                        LeaderID = item.LeaderID,
                        EMail = item.EMail,
                        IsEnabled = item.IsEnabled,
                    };

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary>
        /// 取得 使用者 管理用清單
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<UserModel> GetUserAdminList(Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.Users
                    where
                        item.IsEnabled == "Y"
                    orderby item.UserID
                    select
                    new UserModel()
                    {
                        UserID = item.UserID,
                        EmpID = item.EmpID,
                        FirstNameEN = item.FirstNameEN,
                        LastNameEN = item.LastNameEN,
                        FirstNameCH = item.FirstNameCH,
                        LastNameCH = item.LastNameCH,
                        UnitCode = item.UnitCode,
                        UnitName = item.UnitName,
                        LeaderID = item.LeaderID,
                        EMail = item.EMail,
                        IsEnabled = item.IsEnabled,
                    };

                    var list = query.ProcessPager(pager).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }


        /// <summary> 查詢使用者 </summary>
        /// <param name="userIDs"> 要查詢的使用者 ID </param>
        /// <returns></returns>
        public UserModel GetUser(string userID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.Users
                     where
                        item.UserID == userID && item.IsEnabled == "Y"
                     select
                     new UserModel()
                     {
                         UserID = item.UserID,
                         EmpID = item.EmpID,
                         FirstNameEN = item.FirstNameEN,
                         LastNameEN = item.LastNameEN,
                         FirstNameCH = item.FirstNameCH,
                         LastNameCH = item.LastNameCH,
                         UnitCode = item.UnitCode,
                         UnitName = item.UnitName,
                         LeaderID = item.LeaderID,
                         EMail = item.EMail,
                         IsEnabled = item.IsEnabled,
                     }
                    ).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 查詢使用者的管理者 </summary>
        /// <param name="userIDs"> 使用者帳號 </param>
        /// <returns></returns>
        public List<UserAccountModel> GetUserList_AccountModel(params string[] userIDs)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.Users
                        where
                            userIDs.Contains(item.UserID) && item.IsEnabled == "Y"
                        select
                            new UserAccountModel()
                            {
                                ID = item.UserID,
                                EmpID = item.EmpID,
                                FirstNameEN = item.FirstNameEN,
                                LastNameEN = item.LastNameEN,
                                FirstNameCH = item.FirstNameCH,
                                LastNameCH = item.LastNameCH,
                                UnitCode = item.UnitCode,
                                UnitName = item.UnitName,
                                LeaderID = item.LeaderID,
                                EMail = item.EMail,
                                IsEnable = (item.IsEnabled == "Y"),
                            };

                    var result = query.ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        /// <summary> 查詢使用者 </summary>
        /// <param name="userIDs"> 要查詢的使用者 ID (多筆) </param>
        /// <returns></returns>
        public List<UserModel> GetUserList(IEnumerable<string> userIDs)
        {
            if (userIDs == null || !userIDs.Any())
                return new List<UserModel>();

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.Users
                     where
                        userIDs.Contains(item.UserID) && item.IsEnabled == "Y"
                     select
                     new UserModel()
                     {
                         UserID = item.UserID,
                         EmpID = item.EmpID,
                         FirstNameEN = item.FirstNameEN,
                         LastNameEN = item.LastNameEN,
                         FirstNameCH = item.FirstNameCH,
                         LastNameCH = item.LastNameCH,
                         UnitCode = item.UnitCode,
                         UnitName = item.UnitName,
                         LeaderID = item.LeaderID,
                         EMail = item.EMail,
                         IsEnabled = item.IsEnabled,
                     }
                    ).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 查詢使用者的管理者 </summary>
        /// <param name="userID"> 使用者帳號 </param>
        /// <returns></returns>
        public UserAccountModel GetUserLeader(string userID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.Users
                     where
                        item.IsEnabled == "Y" &&
                        (
                            context.Users.Where(obj => obj.UserID == userID).Select(obj => obj.LeaderID).FirstOrDefault()
                        ).Contains(item.UserID)
                     select
                     new UserAccountModel()
                     {
                         ID = item.UserID,
                         EmpID = item.EmpID,
                         FirstNameEN = item.FirstNameEN,
                         LastNameEN = item.LastNameEN,
                         FirstNameCH = item.FirstNameCH,
                         LastNameCH = item.LastNameCH,
                         UnitCode = item.UnitCode,
                         UnitName = item.UnitName,
                         LeaderID = item.LeaderID,
                         EMail = item.EMail,
                         IsEnable = (item.IsEnabled == "Y"),
                     }
                    ).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 查詢使用者 </summary>
        /// <param name="userID"> 指定的帳號 </param>
        /// <returns></returns>
        public UserModel GetUserAdminDetail(string userID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.Users
                     where
                        item.UserID == userID && item.IsEnabled == "Y"
                     select
                         new UserModel()
                         {
                             UserID = item.UserID,
                             EmpID = item.EmpID,
                             FirstNameEN = item.FirstNameEN,
                             LastNameEN = item.LastNameEN,
                             FirstNameCH = item.FirstNameCH,
                             LastNameCH = item.LastNameCH,
                             UnitCode = item.UnitCode,
                             UnitName = item.UnitName,
                             LeaderID = item.LeaderID,
                             EMail = item.EMail,
                             IsEnabled = item.IsEnabled,
                         }
                    ).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        /// <summary>
        /// 取得 使用者 下拉選單用清單
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<KeyTextModel> GetUserKeyTextList(Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.Users
                        where
                            item.IsEnabled == "Y"
                        orderby item.UserID
                        select new KeyTextModel()
                        {
                            Key = item.EmpID,
                            Text = item.FirstNameEN + " " + item.LastNameEN + " (" + item.EmpID + ") - " + item.UnitName
                        };

                    var list = query.ProcessPager(pager).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        #endregion
    }
}


