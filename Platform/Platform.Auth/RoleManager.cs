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
    public class RoleManager
    {
        private Logger _logger = new Logger();


        #region Read
        /// <summary> 取得角色清單 </summary>
        /// <param name="caption"> 包含文字 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<RoleModel> GetRoleList(string caption, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from obj in context.Roles
                        where
                            obj.IsEnable == true
                        orderby obj.CreateDate descending
                        select new RoleModel()
                        {
                            ID = obj.ID,
                            Name = obj.Name,
                            IsEnable = obj.IsEnable,
                            CreateDate = obj.CreateDate,
                            CreateUser = obj.CreateUser,
                            ModifyDate = obj.ModifyDate,
                            ModifyUser = obj.ModifyUser,
                        };

                    //----- 附加查詢條件 -----
                    if (!string.IsNullOrWhiteSpace(caption))
                        query = query.Where(obj => obj.Name.Contains(caption));
                    //----- 附加查詢條件 -----

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

        /// <summary> 取得角色清單 </summary>
        /// <param name="caption"> 包含文字 </param>
        /// <param name="pager"> 分頁資訊 </param>
        /// <returns></returns>
        public List<RoleModel> GetRoleAdminList(string caption, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from obj in context.Roles
                        orderby obj.CreateDate descending
                        select new RoleModel()
                        {
                            ID = obj.ID,
                            Name = obj.Name,
                            IsEnable = obj.IsEnable,
                            CreateDate = obj.CreateDate,
                            CreateUser = obj.CreateUser,
                            ModifyDate = obj.ModifyDate,
                            ModifyUser = obj.ModifyUser,
                        };

                    //----- 附加查詢條件 -----
                    if (!string.IsNullOrWhiteSpace(caption))
                        query = query.Where(obj => obj.Name.Contains(caption));
                    //----- 附加查詢條件 -----

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

        /// <summary> 取得角色 </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public RoleModel GetDetail(Guid roleID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                       from obj in context.Roles
                       where
                           obj.ID == roleID
                       select new RoleModel()
                       {
                           ID = obj.ID,
                           Name = obj.Name,
                           IsEnable = obj.IsEnable,
                           CreateDate = obj.CreateDate,
                           CreateUser = obj.CreateUser,
                           ModifyDate = obj.ModifyDate,
                           ModifyUser = obj.ModifyUser,
                       };

                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }
        #endregion

        #region CUD
        /// <summary> 建立角色 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public void CreateRole(RoleModel model, string userID, DateTime time)
        {
            List<string> msgList = new List<string>();

            if (!this.ValidModel(model, out msgList))
                throw new ArgumentException(string.Join(", ", msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    Role dbRole = new Role()
                    {
                        ID = Guid.NewGuid(),
                        Name = model.Name,

                        IsEnable = model.IsEnable,
                        CreateUser = userID,
                        CreateDate = DateTime.Now,
                        ModifyUser = userID,
                        ModifyDate = DateTime.Now
                    };

                    context.Roles.Add(dbRole);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 修改角色 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public void ModifyRole(RoleModel model, string userID, DateTime time)
        {
            List<string> msgList = new List<string>();

            if (!this.ValidModel(model, out msgList))
                throw new ArgumentException(string.Join(", ", msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbRole = context.Roles.Where(obj => obj.ID == model.ID).FirstOrDefault();

                    if (dbRole != null)
                    {
                        dbRole.Name = model.Name;
                        dbRole.IsEnable = model.IsEnable;

                        dbRole.ModifyUser = userID;
                        dbRole.ModifyDate = DateTime.Now;
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

        /// <summary> 刪除角色 </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public void DeleteRole(Guid id, string userID, DateTime time)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbRole = context.Roles.Where(obj => obj.ID == id).FirstOrDefault();

                    context.Roles.Remove(dbRole);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region Private
        private bool ValidModel(RoleModel model, out List<string> msgList)
        {
            msgList = new List<string>();

            // 檢查名稱是否空白
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                msgList.Add("RoleName is required.");
            }

            // 檢查名稱是否重覆
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from obj in context.Roles
                        where
                            obj.Name == model.Name
                        select obj;

                    if (model.ID != Guid.Empty)
                        query = query.Where(obj => obj.ID != model.ID);

                    if (query.Any())
                    {
                        msgList.Add("RoleName repeated.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }

            if (msgList.Count > 0)
                return false;
            return true;
        }
        #endregion
    }
}
