using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class StatusTable
    {
        public int Complete { get; set; }
        public int OrderID { get; set; }
        public double SubTotal { get; set; }
        public string TableID { get; set; }
        public int Seat { get; set; }
        public string Time { get; set; }
        public string OrderNum { get; set; }
    }
}
