using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class DailyReportDetailModel
    {
       
        public int InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public int OrderID { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public double Total { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateString { get; set; }



    }
}
