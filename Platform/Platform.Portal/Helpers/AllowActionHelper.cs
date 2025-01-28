using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.Portal.Models;

namespace Platform.Portal.Helpers
{
    internal class AllowActionHelper
    {
        #region Private Methods
        /// <summary> 轉換標準行為 Enum 至標準行為清單 </summary>
        /// <param name="enm"></param>
        /// <returns></returns>
        internal static List<AllowActionEnum> EnumFlagToList(AllowActionEnum enm)
        {
            List<AllowActionEnum> list = new List<AllowActionEnum>();

            if ((enm & AllowActionEnum.ReadList) != 0)
                list.Add(AllowActionEnum.ReadList);

            if ((enm & AllowActionEnum.ReadDetail) != 0)
                list.Add(AllowActionEnum.ReadDetail);

            if ((enm & AllowActionEnum.Create) != 0)
                list.Add(AllowActionEnum.Create);

            if ((enm & AllowActionEnum.Modify) != 0)
                list.Add(AllowActionEnum.Modify);

            if ((enm & AllowActionEnum.Delete) != 0)
                list.Add(AllowActionEnum.Delete);

            if ((enm & AllowActionEnum.Export) != 0)
                list.Add(AllowActionEnum.Export);

            if ((enm & AllowActionEnum.Admin) != 0)
                list.Add(AllowActionEnum.Admin);

            return list;
        }

        /// <summary> 轉換標準行為清單至標準行為 Enum </summary>
        /// <param name=""></param>
        /// <returns></returns>
        internal static AllowActionEnum ListToEnumFlag(List<AllowActionEnum> list)
        {
            AllowActionEnum actions = 0;

            if (list.Contains(AllowActionEnum.ReadList))
                actions |= AllowActionEnum.ReadList;

            if (list.Contains(AllowActionEnum.ReadDetail))
                actions |= AllowActionEnum.ReadDetail;

            if (list.Contains(AllowActionEnum.Create))
                actions |= AllowActionEnum.Create;

            if (list.Contains(AllowActionEnum.Modify))
                actions |= AllowActionEnum.Modify;

            if (list.Contains(AllowActionEnum.Delete))
                actions |= AllowActionEnum.Delete;

            if (list.Contains(AllowActionEnum.Export))
                actions |= AllowActionEnum.Export;

            if (list.Contains(AllowActionEnum.Admin))
                actions |= AllowActionEnum.Admin;

            return actions;
        }
        #endregion
    }
}
