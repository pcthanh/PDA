using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class OrderItem
    {
       public int ProductID { get; set; }
       public int Price { get; set; }
       public int Qty { get; set; }
       public int ChangeStatus { get; set; }
       public int Seat { get; set; }
       public int DynID { get; set; }
       public string ProductName { get; set; }
       public string PrinterID { get; set; }
       public string CategoryID { get; set; }
       public int KeyIndex { get; set; }
    }
}
