using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public partial class ModifirePriceModel
    {
        public int ModifirePriceID { get; set; }
        public int ModifireID { get; set; }
        public int Status { get; set; }
        public Nullable<double> CurrentPrice { get; set; }
        public Nullable<double> WasPrice { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
