using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Try
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var arr = new string[] { "材料" };

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_Supplier
                        where
                            arr.Any(obj => item.BusinessCategory.Contains(obj))
                        select item;

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ToList();


                    foreach (var item in list)
                    {
                        Console.WriteLine(item.BusinessCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
