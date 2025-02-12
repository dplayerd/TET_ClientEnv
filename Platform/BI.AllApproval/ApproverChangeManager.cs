using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.ORM;
using BI.Suppliers.Models;
using Platform.LogService;


namespace BI.AllApproval
{
    public class ApproverChangeManager
    {
        private Logger _logger = new Logger();

        /// <summary> 查詢審核資料 </summary>
        /// <returns></returns>
        public TET_SupplierApprovalModel GetTET_SupplierApproval(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.vwApprovalList
                     where
                        item.ID == ID
                     select
                        new TET_SupplierApprovalModel()
                        {
                            ID = item.ID,
                            //SupplierID = item.SupplierID,
                            Type = item.Type,
                            Description = item.Description,
                            Level = item.Level,
                            Approver = item.Approver,
                            Result = item.Result,
                            Comment = item.Comment,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
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

        /// <summary> 修改審核人 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifyApprover(TET_SupplierApprovalModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 修改前，先檢查是否能通過商業邏輯
            //if (!SupplierSTQAValidator.Valid(model, out List<string> msgList))
            //    throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    if (model.Type == "新增供應商審核" || model.Type == "供應商資訊異動審核")
                    {
                        var dbModel =
                        (from item in context.TET_SupplierApproval 
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                        if (dbModel == null)
                            throw new NullReferenceException($"{model.ID} don't exists.");

                        dbModel.Approver = model.Approver;
                        dbModel.ModifyUser = userID;
                        dbModel.ModifyDate = cDate;

                        context.SaveChanges();
                    }
                    else if (model.Type == "新增SPA資料審核")
                    {
                        var dbModel =
                        (from item in context.TET_SupplierSPAApproval
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                        if (dbModel == null)
                            throw new NullReferenceException($"{model.ID} don't exists.");

                        dbModel.Approver = model.Approver;
                        dbModel.ModifyUser = userID;
                        dbModel.ModifyDate = cDate;

                        context.SaveChanges();
                    }
                    else if (model.Type == "新增STQA資料審核")
                    {
                        var dbModel =
                        (from item in context.TET_SupplierSTQAApproval
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                        if (dbModel == null)
                            throw new NullReferenceException($"{model.ID} don't exists.");

                        dbModel.Approver = model.Approver;
                        dbModel.ModifyUser = userID;
                        dbModel.ModifyDate = cDate;

                        context.SaveChanges();
                    }
                    else if (model.Type == "新增一般付款對象審核" || model.Type == "一般付款對象資訊異動審核")
                    {
                        var dbModel =
                        (from item in context.TET_PaymentSupplierApproval
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                        if (dbModel == null)
                            throw new NullReferenceException($"{model.ID} don't exists.");

                        dbModel.Approver = model.Approver;
                        dbModel.ModifyUser = userID;
                        dbModel.ModifyDate = cDate;

                        context.SaveChanges();
                    }
                    else if (model.Type == "Cost&Service資料審核")
                    {
                        var dbModel =
                        (from item in context.TET_SPA_CostServiceApproval
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                        if (dbModel == null)
                            throw new NullReferenceException($"{model.ID} don't exists.");

                        dbModel.Approver = model.Approver;
                        dbModel.ModifyUser = userID;
                        dbModel.ModifyDate = cDate;

                        context.SaveChanges();
                    }
                    else if (model.Type == "SPA評鑑計分資料審核")
                    {
                        var dbModel =
                        (from item in context.TET_SPA_ScoringInfoApproval
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                        if (dbModel == null)
                            throw new NullReferenceException($"{model.ID} don't exists.");

                        dbModel.Approver = model.Approver;
                        dbModel.ModifyUser = userID;
                        dbModel.ModifyDate = cDate;

                        context.SaveChanges();
                    }
                    else if (model.Type == "違規紀錄資料審核")
                    {
                        var dbModel =
                        (from item in context.TET_SPA_ViolationApproval
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                        if (dbModel == null)
                            throw new NullReferenceException($"{model.ID} don't exists.");

                        dbModel.Approver = model.Approver;
                        dbModel.ModifyUser = userID;
                        dbModel.ModifyDate = cDate;

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
    }
}
