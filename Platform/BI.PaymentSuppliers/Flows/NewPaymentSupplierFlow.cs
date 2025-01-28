using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Flows
{
    /// <summary> 新增供應商的流程 </summary>
    public class NewPaymentSupplierFlow
    {
        private static List<FlowModel> _newPaymentSupplierFlow = new List<FlowModel>()
        {
            new FlowModel() { Level = ApprovalLevel.User_GL, Role = ApprovalRole.User_GL, IsStart = true },
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
        public static FlowModel GetCurrentFlow(TET_PaymentSupplierApprovalModel cApprovalModel, TET_PaymentSupplierModel paymenysupplierModel, string cUserID)
        {
            // 先找出自己的關卡
            var cLevel = cApprovalModel.Level;
            var cFlow = _newPaymentSupplierFlow.Where(obj => obj.Level.ToText() == cLevel).FirstOrDefault();

            if (cFlow == null)
                throw new Exception("Error level name");

            return cFlow;
        }

        /// <summary> 找出下一關 (如果傳入的是最後一關，會回傳 NULL)
        /// <para> (如果傳入的 Level 名稱有錯，會拋錯誤) </para>
        /// </summary>
        /// <param name="cApprovalModel">  </param>
        /// <param name="supplierModel"></param>
        /// <param name="cUserID"></param>
        /// <returns></returns>
        public static FlowModel GetNextFlow(TET_PaymentSupplierApprovalModel cApprovalModel, TET_PaymentSupplierModel paymenysupplierModel, string cUserID)
        {
            FlowModel cFlow = GetCurrentFlow(cApprovalModel, paymenysupplierModel, cUserID);

            // 依關卡決定下一關 
            //   如果是最後一關，回傳 NULL
            var index = _newPaymentSupplierFlow.IndexOf(cFlow);
            if (index >= _newPaymentSupplierFlow.Count - 1)
                return null;

            // 否則回傳下一關
            var newFlow = _newPaymentSupplierFlow[index + 1];
            return newFlow;
        }

        /// <summary> 找出上一關 (如果傳入的是首關，會回傳 NULL)
        /// <para> (如果傳入的 Level 名稱有錯，會拋錯誤) </para>
        /// </summary>
        /// <param name="cApprovalModel">  </param>
        /// <param name="supplierModel"></param>
        /// <param name="cUserID"></param>
        /// <returns></returns>
        public static FlowModel GetPrevFlow(TET_PaymentSupplierApprovalModel cApprovalModel, TET_PaymentSupplierModel paymenysupplierModel, string cUserID)
        {
            // 先找出自己的關卡
            FlowModel cFlow = GetCurrentFlow(cApprovalModel, paymenysupplierModel, cUserID);


            // 依關卡決定下一關 
            //   如果是首一關，回傳 NULL
            var index = _newPaymentSupplierFlow.IndexOf(cFlow);
            if (index <= 0)
                return null;

            // 否則回傳上一關
            var newFlow = _newPaymentSupplierFlow[index - 1];
            return newFlow;
        }
    }
}
