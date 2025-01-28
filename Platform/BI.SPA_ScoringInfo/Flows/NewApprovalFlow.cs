using BI.SPA_ScoringInfo.Enums;
using BI.SPA_ScoringInfo.Models;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Flows
{
    /// <summary> 新增 Cost&Service資料維護 的流程 </summary>
    public class NewApprovalFlow
    {
        private static List<FlowModel> _mainFlow = new List<FlowModel>()
        {
            new FlowModel() { Level = ApprovalLevel.FirstApproval, Role = ApprovalRole.Empty, IsStart = true, IsLast = false },
            new FlowModel() { Level = ApprovalLevel.SecondApproval, Role = ApprovalRole.Empty, IsStart = false, IsLast = false },
            new FlowModel() { Level = ApprovalLevel.QSM, Role = ApprovalRole.QSM, IsStart = false, IsLast = true },
        };

        /// <summary> 取得指定關卡 </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static FlowModel GetTargetFlow(ApprovalLevel lvl)
        {
            return _mainFlow.Where(obj=>obj.Level == lvl).FirstOrDefault();
        }

        /// <summary> 取得目前的關卡 </summary>
        /// <param name="cApprovalModel"></param>
        /// <param name="supplierModel"></param>
        /// <param name="cUserID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static FlowModel GetCurrentFlow(SPA_ScoringInfoApprovalModel cApprovalModel, SPA_ScoringInfoModel supplierModel, string cUserID)
        {
            // 先找出自己的關卡
            var cLevel = cApprovalModel.Level;
            var cFlow = _mainFlow.Where(obj => obj.Level.ToText() == cLevel).FirstOrDefault();

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
        public static FlowModel GetNextFlow(SPA_ScoringInfoApprovalModel cApprovalModel, SPA_ScoringInfoModel supplierModel, string cUserID)
        {
            FlowModel cFlow = GetCurrentFlow(cApprovalModel, supplierModel, cUserID);

            // 依關卡決定下一關 
            //   如果是最後一關，回傳 NULL
            var index = _mainFlow.IndexOf(cFlow);
            if (index >= _mainFlow.Count - 1)
                return null;

            // 否則回傳下一關
            var newFlow = _mainFlow[index + 1];
            return newFlow;
        }

        /// <summary> 找出上一關 (如果傳入的是首關，會回傳 NULL)
        /// <para> (如果傳入的 Level 名稱有錯，會拋錯誤) </para>
        /// </summary>
        /// <param name="cApprovalModel">  </param>
        /// <param name="supplierModel"></param>
        /// <param name="cUserID"></param>
        /// <returns></returns>
        public static FlowModel GetPrevFlow(SPA_ScoringInfoApprovalModel cApprovalModel, SPA_ScoringInfoModel supplierModel, string cUserID)
        {
            // 先找出自己的關卡
            FlowModel cFlow = GetCurrentFlow(cApprovalModel, supplierModel, cUserID);


            // 依關卡決定下一關 
            //   如果是首一關，回傳 NULL
            var index = _mainFlow.IndexOf(cFlow);
            if (index <= 0)
                return null;

            // 否則回傳上一關
            var newFlow = _mainFlow[index - 1];
            return newFlow;
        }
    }
}
