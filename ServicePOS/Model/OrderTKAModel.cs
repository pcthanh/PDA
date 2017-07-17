using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class OrderTKAModel
    {
       public string CusName { get; set; }
       public string CusPhone { get; set; }
       public double Total { get; set; }
       public DateTime Waiting { get; set; }
       public string TKAID { get; set; }
    }
}
