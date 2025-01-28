using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using Platform.Portal.Models;

namespace Platform.Portal
{
    public class ModuleManager
    {
        private Logger _logger = new Logger();

        #region READ
        public ModuleModel GetAdminDetail(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var retObj =
                        (from obj in context.Modules
                         where
                            obj.ID == id &&
                            (obj.DeleteDate == null && obj.DeleteUser == null)
                         select
                             new ModuleModel()
                             {
                                 ID = obj.ID,
                                 Name = obj.Name,
                                 Controller = obj.Controller,
                                 Action = obj.Action,
                                 AdminController = obj.AdminController,
                                 AdminAction = obj.AdminAction,
                                 CreateUser = obj.CreateUser,
                                 CreateDate = obj.CreateDate,
                                 ModifyUser = obj.ModifyUser,
                                 ModifyDate = obj.ModifyDate,
                                 DeleteUser = obj.DeleteUser,
                                 DeleteDate = obj.DeleteDate,
                             }
                        ).FirstOrDefault();

                    return retObj;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        public List<ModuleModel> GetAdminList(Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in BuildQuery(context)
                        orderby item.Name, item.Controller, item.Action
                        select new ModuleModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Controller = item.Controller,
                            Action = item.Action,
                            AdminController = item.AdminController,
                            AdminAction = item.AdminAction,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                            DeleteUser = item.DeleteUser,
                            DeleteDate = item.DeleteDate,
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

        public List<ModuleModel> GetModuleList()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in BuildQuery(context)
                        orderby item.Name, item.Controller, item.Action
                        select new ModuleModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Controller = item.Controller,
                            Action = item.Action,
                            AdminController = item.AdminController,
                            AdminAction = item.AdminAction,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                            DeleteUser = item.DeleteUser,
                            DeleteDate = item.DeleteDate,
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
        #endregion

        #region CUD
        public void CreateModule(ModuleModel model, string cUserID, DateTime time)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException("Module name is required.");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var sameNameQuery = BuildQuery(context, model.Name);

                    if (sameNameQuery.Any())
                        throw new ArgumentException(model.Name + " is exist. Please check.");

                    var dbModel = new Module()
                    {
                        ID = Guid.NewGuid(),
                        Name = model.Name,
                        Action = model.Action,
                        Controller = model.Controller,
                        AdminAction = model.AdminAction,
                        AdminController = model.AdminController,
                        CreateDate = time,
                        CreateUser = cUserID,
                    };

                    context.Modules.Add(dbModel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        public void ModifyModule(ModuleModel model, string cUserID, DateTime time)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException("Module name is required.");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel = BuildGetOneQuery(context, model.ID);

                    if (dbModel == null)
                        throw new NullReferenceException($"Module[ID: {model.ID}] is not exist. Please check.");

                    dbModel.Action = model.Action;
                    dbModel.Controller = model.Controller;
                    dbModel.AdminAction = model.AdminAction;
                    dbModel.AdminController = model.AdminController;
                    dbModel.ModifyDate = time;
                    dbModel.ModifyUser = cUserID;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        public void DeleteModule(Guid id, string cUserID, DateTime time)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel = BuildGetOneQuery(context, id);

                    if (dbModel == null)
                        throw new NullReferenceException($"Module[ID: {id}] is not exist. Please check.");

                    dbModel.DeleteDate = time;
                    dbModel.DeleteUser = cUserID;

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
        private static IQueryable<Module> BuildQuery(PlatformContextModel context, string moduleName = null)
        {
            var query =
                (from obj in context.Modules
                 where
                    (obj.DeleteDate == null && obj.DeleteUser == null)
                 select obj);

            if (!string.IsNullOrWhiteSpace(moduleName))
                query = query.Where(obj => obj.Name == moduleName);

            return query;
        }

        private static Module BuildGetOneQuery(PlatformContextModel context, Guid id)
        {
            var query =
                (from obj in context.Modules
                 where
                     obj.ID == id &&
                     (obj.DeleteDate == null && obj.DeleteUser == null)
                 select obj);

            return query.FirstOrDefault();
        }
        #endregion
    }
}
