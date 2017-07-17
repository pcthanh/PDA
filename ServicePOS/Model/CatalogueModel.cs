using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class CatalogueModel
    {
        public int CatalogueID { get; set; }
        public string CatalogueName { get; set; }
        public int Status { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
