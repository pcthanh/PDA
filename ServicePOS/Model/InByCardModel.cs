using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class InByCardModel
    {
        public string InvoiceByCardID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> CardID { get; set; }
        public Nullable<double> Total { get; set; }
        public string Note { get; set; }
        public int ID { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
