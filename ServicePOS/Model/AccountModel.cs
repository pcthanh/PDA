using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class AccountModel
    {
        public int PaymentID { get; set; }
        public double Total { get; set; }
        public int CusNo { get; set; }
    }
}
