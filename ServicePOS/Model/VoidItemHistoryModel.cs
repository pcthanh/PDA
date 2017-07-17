using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
  public  class VoidItemHistoryModel
    {
        public int ID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> ModifireID { get; set; }
        public Nullable<int> ShiftID { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<int> StaffID { get; set; }
        public Nullable<int> Qty { get; set; }
        public string FloorID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
