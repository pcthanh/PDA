using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class OrderJoinTableModel
    {
        public int OrderID { get; set; }
        public double SubTotalTable { get; set; }
        public int TableID { get; set; }
        public int TableIDNew { get; set; }
        public string OrderNumber { get; set; }
    }
}
