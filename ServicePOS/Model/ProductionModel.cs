using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class ProductionModel
    {
        public int ProductID { get; set; }
        public string ProductNameDesc { get; set; }
        public int Status { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string ProductNameSort { get; set; }
        public Nullable<double> CurrentPrice { get; set; }
        public Nullable<double> WasPrice { get; set; }
        public int CategoryID { get; set; }
        public int Printer { get; set; }
        public int PrinterJob { get; set; }
        public List<PrinteJobDetailModel> ListPrinter = new List<PrinteJobDetailModel>();
    }
}
