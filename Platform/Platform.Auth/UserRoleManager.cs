using System;
using System.Collections.Generic;
using System.Linq;
using Platform.AbstractionClass;
using Platform.Auth.Models;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;

namespace Platform.Auth
{
    /// <summary> 帳號角色管理器 </summary>
    public class UserRoleManager
    {
        private Logger _logger = new Logger();

        private RoleManager _roleMgr = new RoleManager();

        /// <summary> 取得帳號角色清單 </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<UserRoleModel> GetUserRoleList(Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.UserRoles
                        join roleItem in context.Roles on item.RoleID equals roleItem.ID
                        join userItem in context.Users on item.UserID equals userItem.UserID
                        orderby item.CreateDate descending
                        select new UserRoleModel()
                        {
                            ID = item.ID,
                            UserID = item.UserID,
                            RoleID = item.RoleID,
                            Account = userItem.UserID,
                            RoleName = roleItem.Name
                        };

                    var list = query.ProcessPager(pager).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查出角色下的所有帳號 </summary>
        /// <param name="roleID"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<UserAccountModel> GetAssignedRoleUserList(Guid roleID, Pager pager, string name)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var role = this._roleMgr.GetDetail(roleID);
                    if (role == null)
                        throw new Exception($"Role doesn't exist.[ID: {roleID}]");

                    var orgQuery =
                        from item in context.UserRoles
                        join userItem in context.Users on item.UserID equals userItem.UserID
                        where
                            item.RoleID == roleID
                        select userItem;

                    if(!string.IsNullOrWhiteSpace(name))
                        orgQuery = orgQuery.Where(obj => obj.EmpID.Contains(name) || obj.FirstNameCH.Contains(name) || obj.LastNameCH.Contains(name) || obj.FirstNameEN.Contains(name) || obj.LastNameEN.Contains(name) || obj.UnitName.Contains(name));

                    var query =
                        from item in orgQuery
                        orderby item.UserID
                        select new UserAccountModel()
                        {
                            ID = item.UserID,
                            EmpID = item.EmpID,
                            EMail = item.EMail,
                            FirstNameCH = item.FirstNameCH,
                            LastNameCH = item.LastNameCH,
                            FirstNameEN = item.FirstNameEN,
                            LastNameEN = item.LastNameEN,
                            LeaderID = item.LeaderID,
                            UnitCode = item.UnitCode,
                            UnitName = item.UnitName,
                            IsEnable = item.IsEnabled == "Y",
                        };

                    var list = query.ProcessPager(pager).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }


        /// <summary> 查出角色下的所有帳號 </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<UserAccountModel> GetUserListInRole(Guid roleID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var role = this._roleMgr.GetDetail(roleID);

                    if (role == null)
                        throw new Exception($"Role doesn't exist.[ID: {roleID}]");

                    var query =
                        from item in context.UserRoles
                        join userItem in context.Users on item.UserID equals userItem.UserID
                        where
                            item.RoleID == roleID &&
                            userItem.IsEnabled == "Y"
                        orderby userItem.UserID
                        select new UserAccountModel()
                        {
                            ID = userItem.UserID,
                            EmpID = userItem.EmpID,
                            EMail = userItem.EMail,
                            FirstNameCH = userItem.FirstNameCH,
                            LastNameCH = userItem.LastNameCH,
                            FirstNameEN = userItem.FirstNameEN,
                            LastNameEN = userItem.LastNameEN,
                            LeaderID = userItem.LeaderID,
                            UnitCode = userItem.UnitCode,
                            UnitName = userItem.UnitName,
                            IsEnable = userItem.IsEnabled == "Y",
                        };

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查出角色下的所有未綁定帳號 </summary>
        /// <param name="roleID"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<UserAccountModel> GetUnassignedRoleUserList(Guid roleID, Pager pager, string name)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var role = this._roleMgr.GetDetail(roleID);
                    if (role == null)
                        throw new Exception($"Role doesn't exist.[ID: {roleID}]");

                    var orgQuery =
                        from item in context.Users
                        where
                            !(from roleItem in context.UserRoles
                              where
                                 roleItem.RoleID == roleID
                              select roleItem.UserID).Contains(item.UserID)
                        select item;

                    if (!string.IsNullOrWhiteSpace(name))
                        orgQuery = orgQuery.Where(obj => obj.EmpID.Contains(name) || obj.FirstNameCH.Contains(name) || obj.LastNameCH.Contains(name) || obj.FirstNameEN.Contains(name) || obj.LastNameEN.Contains(name) || obj.UnitName.Contains(name));

                    var query =
                        from item in orgQuery
                        orderby item.UserID
                        select new UserAccountModel()
                        {
                            ID = item.UserID,
                            EmpID = item.EmpID,
                            EMail = item.EMail,
                            FirstNameCH = item.FirstNameCH,
                            LastNameCH = item.LastNameCH,
                            FirstNameEN = item.FirstNameEN,
                            LastNameEN = item.LastNameEN,
                            LeaderID = item.LeaderID,
                            UnitCode = item.UnitCode,
                            UnitName = item.UnitName,
                            IsEnable = item.IsEnabled == "Y",
                        };

                    var list = query.ProcessPager(pager).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 是否為指定的角色 </summary>
        /// <param name="userID"></param>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        public bool IsRole(string userID, params Guid[] roleIDs)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query = this.BuildQuery(context, new string[] { userID }, roleIDs);

                    if (query.Any())
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 將帳號和角色設定對應 </summary>
        /// <param name="userIDs"></param>
        /// <param name="roleIDs"></param>
        /// <param name="cUserID"></param>
        /// <param name="time"></param>
        public void MapUserAndRole(IEnumerable<string> userIDs, IEnumerable<Guid> roleIDs, string cUserID, DateTime time)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query = this.BuildQuery(context, userIDs, roleIDs);
                    var existedList = query.Select(obj => new { obj.UserID, obj.RoleID }).ToList();

                    foreach (var uID in userIDs)
                    {
                        foreach (var rID in roleIDs)
                        {
                            var query2 = existedList.Where(obj => obj.UserID == uID && obj.RoleID == rID);
                            if (query2.Any())
                                continue;

                            var newID = Guid.NewGuid();
                            UserRole createObject = new UserRole()
                            {
                                ID = newID,
                                UserID = uID,
                                RoleID = rID,
                                CreateUser = cUserID,
                                CreateDate = time,
                                ModifyUser = cUserID,
                                ModifyDate = time,
                            };

                            context.UserRoles.Add(createObject);
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 將帳號和角色取消對應 </summary>
        /// <param name="userIDs"></param>
        /// <param name="roleIDs"></param>
        /// <param name="cUserID"></param>
        /// <param name="time"></param>
        public void UnmapUserAndRole(IEnumerable<string> userIDs, IEnumerable<Guid> roleIDs, string cUserID, DateTime time)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query = this.BuildQuery(context, userIDs, roleIDs);
                    var existedList = query.ToList();

                    foreach (var uID in userIDs)
                    {
                        foreach (var rID in roleIDs)
                        {
                            var existObj = existedList.Where(obj => obj.UserID == uID && obj.RoleID == rID).FirstOrDefault();
                            if (existObj == null)
                                continue;

                            context.UserRoles.Remove(existObj);
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        #region Private
        public IQueryable<UserRole> BuildQuery(PlatformContextModel context, IEnumerable<string> userIDs, IEnumerable<Guid> roleIDs)
        {
            var query =
                from item in context.UserRoles
                select item;

            if (userIDs.Any())
                query = query.Where(obj => userIDs.Contains(obj.UserID));

            if (roleIDs.Any())
                query = query.Where(obj => roleIDs.Contains(obj.RoleID));

            return query;
        }
        #endregion
    }
}
