using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class SeatModel
    {
        public int ID { get; set; }
        public Nullable<int> Seat { get; set; }
        public string OrderNumber { get; set; }
        public Nullable<int> CreateBY { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int ChangeStatus { get; set; }
    }
}
