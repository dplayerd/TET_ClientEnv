using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.SampleData.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;

namespace BI.SampleData
{
    public class SampleDataManager
    {
        private Logger _logger = new Logger();

        #region ReadData
        /// <summary> 查詢清單 </summary>
        /// <returns></returns>
        public List<SampleDataModel> GetList()
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {

                    var query =
                        from item in dbContext.SampleDataEntities
                        where
                            (item.DeleteUser == null && item.DeleteDate == null) &&
                            item.IsEnable
                        orderby item.CreateDate descending
                        select item;

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

        /// <summary> 查詢清單 </summary>
        /// <param name="filterParameter"> 過濾條件 </param>
        /// <param name="pager"> 分頁資訊 </param>
        /// <returns></returns>
        public List<SampleDataModel> GetList(SampleDataFilterConditions filterParameter, Pager pager)
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {

                    var query =
                        from item in dbContext.SampleDataEntities
                        where
                            item.IsEnable &&
                            (item.DeleteUser == null && item.DeleteDate == null)
                        select item;

                    //----- 附加查詢條件 -----
                    if (filterParameter.ID.HasValue)
                        query = query.Where(obj => obj.Id == filterParameter.ID.Value);

                    if (!string.IsNullOrEmpty(filterParameter.Name))
                        query = query.Where(obj => obj.Name.Contains(filterParameter.Name));

                    if (!string.IsNullOrEmpty(filterParameter.Title))
                        query = query.Where(obj => obj.Title.Contains(filterParameter.Title));

                    if (filterParameter.StartDate.HasValue)
                        query = query.Where(obj => obj.CreateDate >= filterParameter.StartDate.Value);

                    if (filterParameter.EndDate.HasValue)
                        query = query.Where(obj => obj.CreateDate <= filterParameter.EndDate.Value);
                    //----- 附加查詢條件 -----


                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var result = query.ProcessPager(pager).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 查詢單筆 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SampleDataModel GetDetail(int id)
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {

                    var query =
                        from item in dbContext.SampleDataEntities
                        where
                            item.Id == id &&
                            (item.DeleteUser == null && item.DeleteDate == null) &&
                            item.IsEnable
                        select item;

                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region ReadAdminData
        /// <summary> 查詢管理用清單 </summary>
        /// <returns></returns>
        public List<SampleDataModel> GetAdminList()
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {

                    var query =
                    from item in dbContext.SampleDataEntities
                    where
                        (item.DeleteUser == null && item.DeleteDate == null)
                    orderby item.CreateDate descending
                    select item;

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

        /// <summary> 查詢管理用清單 </summary>
        /// <param name="filterParameter"> 過濾條件 </param>
        /// <param name="pager"> 分頁資訊 </param>
        /// <returns></returns>
        public List<SampleDataModel> GetAdminList(SampleDataFilterConditions filterParameter, Pager pager)
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {

                    var query =
                        from item in dbContext.SampleDataEntities
                        where
                            (item.DeleteUser == null && item.DeleteDate == null)
                        select item;

                    //----- 附加查詢條件 -----
                    if (filterParameter.ID.HasValue)
                        query = query.Where(obj => obj.Id == filterParameter.ID.Value);

                    if (!string.IsNullOrEmpty(filterParameter.Name))
                        query = query.Where(obj => obj.Name.Contains(filterParameter.Name));

                    if (!string.IsNullOrEmpty(filterParameter.Title))
                        query = query.Where(obj => obj.Title.Contains(filterParameter.Title));

                    if (filterParameter.StartDate.HasValue)
                        query = query.Where(obj => obj.CreateDate >= filterParameter.StartDate.Value);

                    if (filterParameter.EndDate.HasValue)
                        query = query.Where(obj => obj.CreateDate <= filterParameter.EndDate.Value);
                    //----- 附加查詢條件 -----

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var result = query.ProcessPager(pager).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 查詢單筆 </summary>
        /// <param name="dbContext"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal SampleDataModel GetAdminDetail(FakeDBContext dbContext, int id)
        {
            var query =
                from item in dbContext.SampleDataEntities
                where
                    item.Id == id &&
                    (item.DeleteUser == null && item.DeleteDate == null)
                select item;

            var result = query.FirstOrDefault();
            return result;
        }

        /// <summary> 查詢單筆 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SampleDataModel GetAdminDetail(int id)
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {
                    return this.GetAdminDetail(dbContext, id);
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region CUD
        /// <summary> 新增資料 </summary>
        /// <param name="model"> 原始資料 </param>
        /// <param name="userID"> 建立者帳號代碼 </param>
        /// <param name="cTime"> 建立時間 </param>
        /// <exception cref="Exception"></exception>
        public void Create(SampleDataModel model, string userID, DateTime cTime)
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {
                    if (this.GetAdminDetail(dbContext, model.Id) != null)
                        throw new Exception("ID:" + model.Id + " exists.");

                    var newModel = new SampleDataModel()
                    {
                        Id = this.GetNewID(),
                        Name = model.Name,
                        Title = model.Title,
                        ImageUrl = model.ImageUrl,
                        IsEnable = true,
                        CreateUser = userID,
                        CreateDate = cTime
                    };

                    dbContext.SampleDataEntities.Add(newModel);
                    dbContext.SaveChanges();

                    model.Id = newModel.Id;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 編輯資料 </summary>
        /// <param name="model"></param>
        /// <param name="userID"> 修改者帳號代碼 </param>
        /// <param name="cTime"> 修改時間 </param>
        /// <exception cref="NullReferenceException"></exception>
        public void Modify(SampleDataModel model, string userID, DateTime cTime)
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {
                    var dbModel = this.GetAdminDetail(dbContext, model.Id);

                    if (dbModel == null)
                        throw new NullReferenceException("ID:" + model.Id + " doesn't exist.");


                    dbModel.Name = model.Name;
                    dbModel.Title = model.Title;
                    dbModel.ImageUrl = model.ImageUrl;
                    dbModel.IsEnable = model.IsEnable;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cTime;

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 刪除資料 </summary>
        /// <param name="id"> 主鍵 </param>
        /// <param name="userID"> 刪除者帳號代碼 </param>
        /// <param name="cTime"> 刪除時間 </param>
        /// <exception cref="NullReferenceException"></exception>
        public void Delete(int id, string userID, DateTime cTime)
        {
            try
            {
                using (var dbContext = new FakeDBContext())
                {
                    var dbModel = this.GetAdminDetail(dbContext, id);

                    if (dbModel == null)
                        throw new NullReferenceException("ID:" + id + " doesn't exist.");

                    dbModel.DeleteUser = userID;
                    dbModel.DeleteDate = cTime;

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        private int GetNewID()
        {
            int result = new FakeDBContext().SampleDataEntities.Select(obj => obj.Id).Max() + 1;
            return result;
        }
    }
}
