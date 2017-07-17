using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class AccountPaymentModel
    {
        public int PayMentID { get; set; }
        public Nullable<int> CusNo { get; set; }
        public Nullable<int> SubTotal { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<int> Cash { get; set; }
        public Nullable<int> Card { get; set; }
        public Nullable<int> IsCredit { get; set; }
        public Nullable<int> IsDebit { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
