using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class PayMentModel
    {
        public int PaymentHistoryID { get; set; }
        public string InvoiceNumber { get; set; }
        public int PaymentTypeID { get; set; }
        public double Total { get; set; }
        public int Satust { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Note { get; set; }
       
    }
}
