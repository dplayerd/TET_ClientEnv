using System;
using System.Collections.Generic;
using System.Linq;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using BI.Suppliers.Models;
using Platform.Auth;

namespace BI.Suppliers
{
    public class SupplierTradeManager
    {
        private Logger _logger = new Logger();


        #region Read
        /// <summary>
        /// 取得 供應商聯絡人 清單
        /// </summary>
        /// <param name="context">  </param>
        /// <param name="vendorCodes"> 供應商代碼 </param>
        /// <returns></returns>
        public List<TET_SupplierTradeModel> GetTET_SupplierTradeList(IEnumerable<string> vendorCodes)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SupplierTrade
                        where vendorCodes.Contains(item.VenderCode)
                        orderby item.CreateDate
                        select
                        new TET_SupplierTradeModel()
                        {
                            SubpoenaNo = item.SubpoenaNo,
                            SubpoenaDate = item.SubpoenaDate,
                            VenderCode = item.VenderCode,
                            Currency = item.Currency,
                            Amount = item.Amount,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
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

        /// <summary> 取得該供應商交易的最後一個年份 </summary>
        /// <param name="vendorCode"></param>
        /// <returns></returns>
        public List<GroupedTradeModel> GetGroupedTradeList(string vendorCode, bool isSS)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SupplierTrade
                        where item.VenderCode == vendorCode
                        select item;

                    var groupedQuery =
                        from item in query
                        group item by new { item.SubpoenaDate.Year, item.Currency } into temp
                        orderby temp.Key.Year descending, temp.Key.Currency
                        select new GroupedTradeModel()
                        {
                            Year = temp.Key.Year,
                            Currency = temp.Key.Currency,
                            TotalAmount = temp.Sum(obj => obj.Amount)
                        };

                    var result = groupedQuery.ToList();

                    if (!isSS)
                    {
                        if (result.Count > 0)
                            result = result.Take(1).ToList();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 取得該供應商交易的最後一個年份 </summary>
        /// <param name="vendorCode"></param>
        /// <returns></returns>
        public int? GetLastYearOfTrade(string vendorCode)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SupplierTrade
                        where item.VenderCode == vendorCode
                        orderby item.SubpoenaDate.Year descending
                        select item;

                    var result = query.FirstOrDefault();
                    if (result == null)
                        return null;

                    return result.SubpoenaDate.Year;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return null;
            }
        }
        #endregion

        #region CUD
        /// <summary> 新增 供應商聯絡人 </summary>
        /// <param name="modelList"> 供應商聯絡人 List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void WriteTET_SupplierTrade(List<TET_SupplierTradeModel> modelList, string userID, DateTime cDate)
        {
            if (modelList == null)
                throw new ArgumentNullException("Trade is required.");

            var SubpoenaNoList = modelList.Select(obj => obj.SubpoenaNo).ToList();

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 透過傳票編號查出資料，如果存在資料庫中就調整值，如果不存在，就新增資料
                    var dbList = context.TET_SupplierTrade.Where(obj => SubpoenaNoList.Contains(obj.SubpoenaNo)).ToList();
                    foreach (var model in modelList)
                    {
                        var dbModel = dbList.Where(obj => obj.SubpoenaNo == model.SubpoenaNo).FirstOrDefault();

                        if (dbModel != null)
                        {
                            dbModel.SubpoenaDate = model.SubpoenaDate;
                            dbModel.VenderCode = model.VenderCode;
                            dbModel.Currency = model.Currency;
                            dbModel.Amount = model.Amount;

                            dbModel.ModifyUser = userID;
                            dbModel.ModifyDate = cDate;
                        }
                        else
                        {
                            var entity = new TET_SupplierTrade()
                            {
                                SubpoenaNo = model.SubpoenaNo,
                                SubpoenaDate = model.SubpoenaDate,
                                VenderCode = model.VenderCode,
                                Currency = model.Currency,
                                Amount = model.Amount,

                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };
                            context.TET_SupplierTrade.Add(entity);
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
