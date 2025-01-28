using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 已刪除介面 </summary>
    public interface IDeleteQuery
    {
        Guid? DeleteUser { get; set; }
        DateTime? DeleteDate { get; set; }
    }

    /// <summary> 允許顯示介面 </summary>
    public interface IEnabledQuery
    {
        bool IsEnable { get; set; }
    }
}
