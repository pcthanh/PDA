using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class ProductPriceModel
    {
        public int ProductPriceID { get; set; }
        public int ProductID { get; set; }
        public int Status { get; set; }
        public Nullable<double> CurrentPrice { get; set; }
        public Nullable<double> WasPrice { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Portions { get; set; }
        public string ProductNameDesc { get; set; }
    }
}
