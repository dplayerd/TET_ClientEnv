using BI.Shared.Models;
using Platform.AbstractionClass;
using Platform.LogService;
using Platform.Infra;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.Shared.Validators;

namespace BI.Shared
{
    public class TET_ParametersManager
    {
        private Logger _logger = new Logger();

        #region Other Read
        /// <summary> 取得 共用參數 清單</summary>
        /// <param name="typeName"> 種類 </param>
        /// <returns></returns>
        public List<KeyTextModel> GetParametersKeyTextList(string typeName)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_Parameters
                    where
                        string.Compare(typeName, item.Type, true) == 0 &&
                        item.IsEnable == true
                    orderby
                        item.Seq ascending
                    select
                        new KeyTextModel()
                        {
                            Key = item.Item,
                            Text = item.Item,
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

        /// <summary> 取得 共用參數 種類清單 </summary>
        /// <param name="typeName"> 種類 </param>
        /// <returns></returns>
        public List<string> GetTET_ParametersTypeList()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_Parameters
                        where
                            item.IsEnable == true
                        orderby
                            item.Type ascending
                        select
                            item.Type;

                    var list = query.Distinct().ToList();
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

        #region Read
        /// <summary> 取得 共用參數 清單 </summary>
        /// <returns></returns>
        public List<TET_ParametersModel> GetAllParametersKeyTextList()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_Parameters
                        where item.IsEnable == true
                        orderby item.Seq ascending
                        select item;

                    var query = this.ConvertToModel(baseQuery);
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

        /// <summary> 取得 共用參數 清單 </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public List<TET_ParametersModel> GetTET_ParametersList(string typeName)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_Parameters
                        where string.Compare(typeName, item.Type, true) == 0
                        orderby item.Seq ascending
                        select item;

                    var query = ConvertToModel(baseQuery);
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

        /// <summary> 取得 共用參數 Item 欄位的值  </summary>
        /// <param name="id"> 共用參數 代碼 </param>
        /// <returns></returns>
        public string GetItem(Guid id)
        {
            return this.GetOne(id)?.Item;
        }

        /// <summary> 取得 共用參數 Item 欄位的值  </summary>
        /// <param name="id"> 共用參數 代碼 </param>
        /// <returns></returns>
        public string GetItem(string typeName, string itemName)
        {
            return this.GetOne(typeName, itemName)?.ID.ToString().ToUpper();
        }

        /// <summary> 取得 共用參數  </summary>
        /// <param name="id"> 共用參數 代碼 </param>
        /// <returns></returns>
        public TET_ParametersModel GetOne(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_Parameters
                        where item.ID == id
                        select item;

                    var query = this.ConvertToModel(baseQuery);
                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 取得 共用參數  </summary>
        /// <param name="typeName"> Type 欄位的值 </param>
        /// <param name="itemName"> Item 欄位的值 </param>
        /// <returns></returns>
        public TET_ParametersModel GetOne(string typeName, string itemName)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_Parameters
                        where
                            string.Compare(typeName, item.Type, true) == 0 &&
                            string.Compare(itemName, item.Item, true) == 0
                        select item;

                    var query = this.ConvertToModel(baseQuery);
                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }


        private IQueryable<TET_ParametersModel> ConvertToModel(IQueryable<TET_Parameters> items)
        {
            return items.Select(item => new TET_ParametersModel()
            {
                ID = item.ID,
                Type = item.Type,
                Item = item.Item,
                Seq = item.Seq,
                IsEnable = item.IsEnable,
                CreateUser = item.CreateUser,
                CreateDate = item.CreateDate,
                ModifyUser = item.ModifyUser,
                ModifyDate = item.ModifyDate,
            });
        }
        #endregion

        #region CUD
        /// <summary> 寫入參數清單 </summary>
        /// <param name="list"> 參數清單 List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void WriteParameterList(List<TET_ParametersModel> list, string userID, DateTime cDate)
        {
            if (list == null)
                throw new ArgumentNullException("Parameter is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!ParameterValidator.Valid(list, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 類型清單
                    var typeList = list.Select(obj => obj.Type).Distinct();

                    // 先找出目前有的
                    var currentList =
                        (from item in context.TET_Parameters
                         where typeList.Contains(item.Type)
                         select item).ToList();


                    foreach (var model in list)
                    {
                        var dbModel =
                            (from item in currentList
                             where
                                item.ID == model.ID
                             select item).FirstOrDefault();

                        // 如果已存在，直接更新
                        if (dbModel != null)
                        {
                            dbModel.Item = model.Item;
                            dbModel.Seq = model.Seq;
                            dbModel.IsEnable = model.IsEnable;
                        }
                        else  // 如果不存在，就新增
                        {
                            dbModel = new TET_Parameters()
                            {
                                ID = Guid.NewGuid(),

                                Type = model.Type,
                                Item = model.Item,
                                Seq = model.Seq,
                                IsEnable = model.IsEnable,

                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };
                            context.TET_Parameters.Add(dbModel);
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion
    }
}