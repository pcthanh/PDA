using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class ShiftHistoryModel
    {
        public int ShiftHistoryID { get; set; }
        public Nullable<int> StaffID { get; set; }
        public string ShiftName { get; set; }
        public Nullable<System.DateTime> StartShift { get; set; }
        public Nullable<System.DateTime> EndShift { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<double> CashStart { get; set; }
        public Nullable<double> CashEnd { get; set; }
        public Nullable<double> SafeDrop { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public string UserName { get; set; }
    }
}
