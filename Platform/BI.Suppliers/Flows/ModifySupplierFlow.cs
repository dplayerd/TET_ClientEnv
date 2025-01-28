using BI.Suppliers.Enums;
using BI.Suppliers.Models;
using BI.Suppliers.Utils;
using Platform.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Flows
{
    public class ModifySupplierFlow
    {
        private static UserRoleManager _userRoleMgr = new UserRoleManager();

        private static List<FlowModel> _noSRI_SS_SupplierFlow = new List<FlowModel>()
        {
            new FlowModel() { Level = ApprovalLevel.User_GL, Role = ApprovalRole.User_GL, IsStart = true },
            new FlowModel() { Level = ApprovalLevel.SRI_SS, Role = ApprovalRole.SRI_SS_Approval },
            new FlowModel() { Level = ApprovalLevel.SRI_SS_GL, Role = ApprovalRole.SRI_SS_GL },
            new FlowModel() { Level = ApprovalLevel.ACC_First, Role = ApprovalRole.ACC_First },
            new FlowModel() { Level = ApprovalLevel.ACC_Second, Role = ApprovalRole.ACC_Second },
            new FlowModel() { Level = ApprovalLevel.ACC_Last, Role = ApprovalRole.ACC_Last, IsLast = true },
        };

        private static List<FlowModel> _withSRI_SS_SupplierFlow = new List<FlowModel>()
        {
            new FlowModel() { Level = ApprovalLevel.SRI_SS_GL, Role = ApprovalRole.SRI_SS_GL, IsStart = true },
            new FlowModel() { Level = ApprovalLevel.ACC_First, Role = ApprovalRole.ACC_First },
            new FlowModel() { Level = ApprovalLevel.ACC_Second, Role = ApprovalRole.ACC_Second },
            new FlowModel() { Level = ApprovalLevel.ACC_Last, Role = ApprovalRole.ACC_Last, IsLast = true },
        };

        /// <summary> 取得目前的關卡 </summary>
        /// <param name="cApprovalModel"></param>
        /// <param name="supplierModel"></param>
        /// <param name="cUserID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static FlowModel GetCurrentFlow(TET_SupplierApprovalModel cApprovalModel, TET_SupplierModel supplierModel, string cUserID)
        {
            // 先找出自己的關卡
            var cLevel = cApprovalModel.Level;

            // 轉換為 RevisionType
            var revisionType = ApprovalUtils.ParseRevisionType(supplierModel.RevisionType);
            if (revisionType == RevisionType.Empty)
                throw new Exception(ApprovalUtils.ParseRevisionTypeError);

            if (IsSriSs(supplierModel))
            {
                var cFlow = _withSRI_SS_SupplierFlow.Where(obj => obj.Level.ToText() == cLevel).FirstOrDefault();

                if (cFlow == null)
                    throw new Exception("Error level name");

                // 如果在 SRI_SS_GL 這關，而且沒有變更銀行資料，這就是最後一關了
                if (cFlow.Level == ApprovalLevel.SRI_SS_GL &&
                    revisionType == RevisionType.Same)
                {
                    cFlow = cFlow.ToCopy();
                    cFlow.IsLast = true;
                }

                return cFlow;
            }
            else
            {
                var cFlow = _noSRI_SS_SupplierFlow.Where(obj => obj.Level.ToText() == cLevel).FirstOrDefault();

                if (cFlow == null)
                    throw new Exception("Error level name");

                // 如果在 SRI_SS_GL 這關，而且沒有變更銀行資料，這就是最後一關了
                if (cFlow.Level == ApprovalLevel.SRI_SS_GL &&
                    revisionType == RevisionType.Same)
                {
                    cFlow = cFlow.ToCopy();
                    cFlow.IsLast = true;
                }


                return cFlow;
            }
        }

        /// <summary> 找出下一關 (如果傳入的是最後一關，會回傳 NULL)
        /// <para> (如果傳入的 Level 名稱有錯，會拋錯誤) </para>
        /// </summary>
        /// <param name="cApprovalModel">  </param>
        /// <param name="supplierModel"></param>
        /// <param name="cUserID"></param>
        /// <returns></returns>
        public static FlowModel GetNextFlow(TET_SupplierApprovalModel cApprovalModel, TET_SupplierModel supplierModel, string cUserID)
        {
            FlowModel cFlow = GetCurrentFlow(cApprovalModel, supplierModel, cUserID);

            // 如果是最後一關，回傳 NULL
            if (cFlow.IsLast)
                return null;

            if (IsSriSs(supplierModel))
            {
                // 依關卡決定下一關 
                //   如果是最後一關，回傳 NULL
                var index = _withSRI_SS_SupplierFlow.IndexOf(cFlow);
                if (index >= _withSRI_SS_SupplierFlow.Count - 1)
                    return null;

                // 否則回傳下一關
                var newFlow = _withSRI_SS_SupplierFlow[index + 1];
                return newFlow;
            }
            else
            {
                // 依關卡決定下一關 
                //   如果是最後一關，回傳 NULL
                var index = _noSRI_SS_SupplierFlow.IndexOf(cFlow);
                if (index >= _noSRI_SS_SupplierFlow.Count - 1)
                    return null;

                // 否則回傳下一關
                var newFlow = _noSRI_SS_SupplierFlow[index + 1];
                return newFlow;
            }
        }

        /// <summary> 找出上一關 (如果傳入的是首關，會回傳 NULL)
        /// <para> (如果傳入的 Level 名稱有錯，會拋錯誤) </para>
        /// </summary>
        /// <param name="cApprovalModel">  </param>
        /// <param name="supplierModel"></param>
        /// <param name="cUserID"></param>
        /// <returns></returns>
        public static FlowModel GetPrevFlow(TET_SupplierApprovalModel cApprovalModel, TET_SupplierModel supplierModel, string cUserID)
        {
            // 先找出自己的關卡
            FlowModel cFlow = GetCurrentFlow(cApprovalModel, supplierModel, cUserID);

            // 如果是第一關，回傳 NULL
            if(cFlow.IsStart) 
                return null;

            if (IsSriSs(supplierModel))
            {
                // 依關卡決定下一關 
                //   如果是首一關，回傳 NULL
                var index = _withSRI_SS_SupplierFlow.IndexOf(cFlow);
                if (index <= 0)
                    return null;

                // 否則回傳上一關
                var newFlow = _withSRI_SS_SupplierFlow[index - 1];
                return newFlow;
            }
            else
            {
                // 依關卡決定下一關 
                //   如果是首一關，回傳 NULL
                var index = _noSRI_SS_SupplierFlow.IndexOf(cFlow);
                if (index <= 0)
                    return null;

                // 否則回傳上一關
                var newFlow = _noSRI_SS_SupplierFlow[index - 1];
                return newFlow;
            }
        }

        private static bool IsSriSs(TET_SupplierModel model)
        {
            if (_userRoleMgr.IsRole(model.CreateUser, ApprovalRole.SRI_SS.ToID().Value, ApprovalRole.SRI_SS_Approval.ToID().Value))
                return true;
            else
                return false;
        }
    }
}
