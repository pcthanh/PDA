using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class OrderOpenItemModel
    {
        public int dynID { get; set; }
        public string ItemNameShort { get; set; }
        public string ItemNameDesc { get; set; }
        public int UnitPrice { get; set; }
        public string PrinterID { get; set; }
        public string PrintType { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
