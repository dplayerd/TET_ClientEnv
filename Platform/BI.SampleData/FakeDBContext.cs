using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.SampleData.Models;

namespace BI.SampleData
{
    /// <summary> 假資料，請視為這裡就是資料庫物件 </summary>
    public class FakeDBContext : IDisposable
    {
        private static List<SampleDataModel> _sourceList { get; set; }

        #region Fake Method
        public void Dispose()
        {
        }

        public void SaveChanges()
        { 
        }
        #endregion


        internal List<SampleDataModel> SampleDataEntities
        {
            get
            {
                // 應該讀 DB ，但為求簡化，所以使用記憶體資料
                if (FakeDBContext._sourceList == null)
                {
                    var list = new List<SampleDataModel>();

                    for (int i = 1; i <= 50; i++)
                    {
                        list.Add(new SampleDataModel()
                        {
                            Id = i,
                            Name = "Name_" + i,
                            Title = "Title_" + i,
                            ImageUrl = "/Content/assets/media/users/blank.png",

                            CreateDate = DateTime.Now.AddDays(-20).AddDays(i),
                            IsEnable = true,
                        });
                    }
                    FakeDBContext._sourceList = list;
                }

                return FakeDBContext._sourceList;
            }
        }
    }
}
