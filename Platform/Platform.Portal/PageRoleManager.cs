using System;
using System.Collections.Generic;
using System.Linq;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using Platform.Portal.Models;

namespace Platform.Portal
{
    public class PageRoleManager
    {
        private Logger _logger = new Logger();
        private RoleManager _roleMgr = new RoleManager();

        /// <summary> 透過帳號代碼查詢可以讀取的頁面代碼 </summary>
        /// <param name="userID"> 帳號代碼 </param>
        /// <returns></returns>
        public List<Guid> GetPageIDListByUserID(string userID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var actType = (byte)AllowActionEnum.ReadList;


                    var rolesOfUserQuery =
                        from item in context.UserRoles
                        where
                          item.UserID == userID
                        select item.RoleID;

                    var rolesOfRoleList = rolesOfUserQuery.ToList();


                    var query =
                        (from item in context.PageRoles
                         where
                             rolesOfRoleList.Contains(item.RoleID) &&
                             (item.AllowActs & actType) == actType
                         select item.MenuID);

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

        /// <summary> 取得頁面角色清單 </summary>
        /// <param name="pager"></param>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public List<PageRoleModel> GetPageRoleList(Pager pager, Guid pageID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from roleItem in context.Roles
                        join item in context.PageRoles.Where(obj => obj.MenuID == pageID) on roleItem.ID equals item.RoleID into PageRoleGroup
                        from pageRoleDefault in PageRoleGroup.DefaultIfEmpty()
                        join pageItem in context.Pages on pageRoleDefault.MenuID equals pageItem.ID into PageGroup
                        from pageDefault in PageGroup.DefaultIfEmpty()
                        orderby pageRoleDefault.CreateDate descending
                        select new
                        {
                            RoleID = roleItem.ID,
                            AllowActs = (pageRoleDefault == null) ? (byte)0 : pageRoleDefault.AllowActs,

                            RoleName = roleItem.Name,
                            PageName = (pageDefault == null) ? null : pageDefault.Name,
                        };


                    var list = query.ProcessPager(pager).ToList();
                    var retList = list.Select(obj =>
                        new PageRoleModel()
                        {
                            PageID = pageID,
                            RoleID = obj.RoleID,
                            AllowActs = obj.AllowActs,
                            RoleName = obj.RoleName,
                            PageName = obj.PageName,
                        }).ToList();

                    ReadPageFunctionRoles(context, retList, pageID);   // 讀取特殊權限
                    return retList;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }


        /// <summary> 取得頁面角色清單 </summary>
        /// <param name="pager"></param>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public List<PageRoleModel> GetRolePageList(Pager pager, Guid roleID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from pageItem in context.Pages
                        join item in context.PageRoles.Where(obj => obj.RoleID == roleID) on pageItem.ID equals item.MenuID into PageRoleGroup
                        from pageRoleDefault in PageRoleGroup.DefaultIfEmpty()
                        join roleItem in context.Roles.Where(obj => obj.ID == roleID) on pageRoleDefault.RoleID equals roleItem.ID into RoleGroup
                        from roleDefault in RoleGroup.DefaultIfEmpty()
                        orderby pageItem.SortNo 
                        select new
                        {
                            RoleID = roleID,
                            PageID = pageItem.ID,
                            AllowActs = (pageRoleDefault == null) ? (byte)0 : pageRoleDefault.AllowActs,

                            RoleName = (roleDefault == null) ? null : roleDefault.Name,
                            PageName = pageItem.Name,
                        };


                    var list = query.ProcessPager(pager).ToList();
                    var retList = list.Select(obj =>
                        new PageRoleModel()
                        {
                            PageID = obj.PageID,
                            RoleID = obj.RoleID,
                            AllowActs = obj.AllowActs,
                            RoleName = obj.RoleName,
                            PageName = obj.PageName,
                        }).ToList();

                    //ReadPageFunctionRoles(context, retList, pageID);   // 讀取特殊權限
                    return retList;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 取得頁面角色 </summary>
        /// <param name="pageID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public PageRoleModel GetPageRole(Guid pageID, Guid roleID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from roleItem in context.Roles
                        join item in context.PageRoles.Where(item2 => item2.MenuID == pageID && item2.RoleID == roleID) on roleItem.ID equals item.RoleID into PageRoleGroup
                        from pageRoleDefault in PageRoleGroup.DefaultIfEmpty()
                        join pageItem in context.Pages on pageRoleDefault.MenuID equals pageItem.ID into PageGroup
                        from pageDefault in PageGroup.DefaultIfEmpty()
                        where
                            roleItem.ID == roleID
                        orderby pageRoleDefault.CreateDate descending
                        select new PageRoleModel
                        {
                            RoleID = roleItem.ID,
                            AllowActs = (pageRoleDefault == null) ? (byte)0 : pageRoleDefault.AllowActs,

                            RoleName = roleItem.Name,
                            PageName = (pageDefault == null) ? null : pageDefault.Name,
                        };

                    var obj = query.FirstOrDefault();
                    obj.PageID = pageID;

                    if (obj == null)
                        return null;

                    ReadPageFunctionRoles(context, new PageRoleModel[] { obj }, pageID);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 將帳號和角色設定對應 </summary>
        /// <param name="list"></param>
        /// <param name="cUserID"></param>
        /// <param name="time"></param>
        public void MapPageAndRole(List<PageRoleModel> list, string cUserID, DateTime time)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    foreach (var model in list)
                    {
                        var query_PageRole = this.BuildQuery(context, model.PageID, model.RoleID);

                        // 若頁面角色權限不存在，就新增，否則更新
                        var dbPageRole = query_PageRole.FirstOrDefault();
                        if (dbPageRole == null)
                        {
                            dbPageRole = new PageRole()
                            {
                                ID = Guid.NewGuid(),
                                MenuID = model.PageID,
                                RoleID = model.RoleID,
                                AllowActs = model.AllowActs,
                                CreateDate = time,
                                CreateUser = cUserID
                            };

                            context.PageRoles.Add(dbPageRole);
                        }
                        else
                        {
                            dbPageRole.AllowActs = model.AllowActs;
                            dbPageRole.ModifyDate = time;
                            dbPageRole.ModifyUser = cUserID;
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

        /// <summary> 取得本頁中被授權的行為 </summary>
        /// <param name="pageID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public PermissionModel GetPermission(Guid pageID, string userID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var userRoleQuery =
                        from item in context.UserRoles
                        where
                            item.UserID == userID
                        select item.RoleID;

                    var pageRoleQuery =
                        from item in context.PageRoles
                        where
                            item.MenuID == pageID &&
                            userRoleQuery.Contains(item.RoleID)
                        select item.AllowActs;

                    var pageFunctionRoleQuery =
                        from item in context.PageFunctionRoles
                        where
                            item.PageID == pageID &&
                            userRoleQuery.Contains(item.RoleID) &&
                            (item.DeleteDate == null && item.DeleteUser == null)
                        select item.FunctionCode;

                    // 如果同一頁面被綁定多個角色，則取聯集
                    byte actByte = 0;
                    var actList = pageRoleQuery.ToList();
                    actList.ForEach(obj => actByte |= obj);

                    // 把所有的特殊行為都取出
                    var funcList = pageFunctionRoleQuery.ToList();

                    PermissionModel retObj = new PermissionModel()
                    {
                        AllowAction = (AllowActionEnum)actByte,
                        Functions = funcList
                    };

                    return retObj;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }


        #region Private
        /// <summary> 組基本查詢 </summary>
        /// <param name="context"></param>
        /// <param name="pageID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private IQueryable<PageRole> BuildQuery(PlatformContextModel context, Guid pageID, Guid roleID)
        {
            var query =
                from item in context.PageRoles
                where
                    item.MenuID == pageID &&
                    item.RoleID == roleID
                select item;

            return query;
        }

        // <summary> 組基本查詢 </summary>
        /// <param name="context"></param>
        /// <param name="pageID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private IQueryable<PageFunctionRole> BuildFunctionQuery(PlatformContextModel context, Guid pageID, Guid roleID)
        {
            var query =
                from item in context.PageFunctionRoles
                where
                    item.PageID == pageID &&
                    item.RoleID == roleID &&
                    (item.DeleteDate == null && item.DeleteUser == null)
                select item;

            return query;
        }


        /// <summary> 讀取特殊權限 </summary>
        /// <param name="context"></param>
        /// <param name="source"></param>
        /// <param name="pageID"></param>
        private void ReadPageFunctionRoles(PlatformContextModel context, IEnumerable<PageRoleModel> source, Guid pageID)
        {
            var query =
                from item in context.PageFunctionRoles
                where
                    item.PageID == pageID &&
                    (item.DeleteDate == null && item.DeleteUser == null)
                select item;

            var list = query.ToList();

            foreach (var item in source)
            {
                var funcRoleList =
                    from obj in list
                    where
                        obj.RoleID == item.RoleID &&
                        obj.PageID == item.PageID
                    select new SpesficAction()
                    {
                        ID = obj.ID,
                        FunctionCode = obj.FunctionCode,
                        IsAllow = obj.IsAllow
                    };

                item.SpesficActionList = funcRoleList.ToList();
            }
        }
        #endregion
    }
}
