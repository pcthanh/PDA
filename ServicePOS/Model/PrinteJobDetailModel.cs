using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class PrinteJobDetailModel
    {
        public int ID { get; set; }
        public int PrinteJobID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> PrinterID { get; set; }
        public Nullable<int> TemplatesID { get; set; }
        public Nullable<int> Status { get; set; }
        public string Notes { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
