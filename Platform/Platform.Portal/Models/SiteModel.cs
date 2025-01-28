using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Portal.Models
{
    public class SiteModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public Guid? MediaFileID { get; set; }
        public string HeaderText { get; set; }
        public string FooterText { get; set; }
    }
}
