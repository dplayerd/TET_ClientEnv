using BI.Suppliers.Enums;
using BI.Suppliers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Flows
{
    public class SupplierFlow
    {
        private static List<FlowModel> _newSupplierFlow = new List<FlowModel>()
        {
            new FlowModel() { Level = ApprovalLevel.User_GL, Role = ApprovalRole.User_GL, IsStart = true },
            new FlowModel() { Level = ApprovalLevel.SRI_SS, Role = ApprovalRole.SRI_SS },
            new FlowModel() { Level = ApprovalLevel.SRI_SS_GL, Role = ApprovalRole.SRI_SS_GL },
            new FlowModel() { Level = ApprovalLevel.ACC_First, Role = ApprovalRole.ACC_First },
            new FlowModel() { Level = ApprovalLevel.ACC_Second, Role = ApprovalRole.ACC_Second },
            new FlowModel() { Level = ApprovalLevel.ACC_Last, Role = ApprovalRole.ACC_Last, IsLast = true },
        };

        public static List<FlowModel> GetNewSupplierFlows()
        {
            return _newSupplierFlow;
        }

        public static FlowModel GetNextFlow(TET_SupplierApprovalManager currentApprovalModel, TET_SupplierModel supplierModel, string cUserID)
        {
            return null;
        }

        public static FlowModel GetPrevFlow(TET_SupplierApprovalManager currentApprovalModel, TET_SupplierModel supplierModel, string cUserID)
        {
            return null;
        }
    }
}
