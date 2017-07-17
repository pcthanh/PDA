using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class InvoiceDetailModifire
    {
        public int InvoiceID { get; set; }
        public int InvoiceDetailID { get; set; }
        public int InvoiceDetailMofifireID { get; set; }
        public int OrderModifireID { get; set; }
        public int OrderDetailID { get; set; }
        public int ModifireID { get; set; }
        public string ModifireName { get; set; }
        public int Satust { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Qty { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Note { get; set; }
        public int KeyItem { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int KeyModi { get; set; }
        public int Seat { get; set; }
        public int ChangeStatus { get; set; }
        public int InvoiceNumber { get; set; }
    }
}
