using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class DiscountModel
    {
       public int DiscountType { get; set; }
       public string DiscountName { get; set; }
       public int Total { get; set; }
    }
}
