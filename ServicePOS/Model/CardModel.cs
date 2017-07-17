using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
  public  class CardModel
    {

      public int PayMenyID { get; set; }
        public int CardID { get; set; }
        public string CardName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> SurChart { get; set; }
        public double SubTotal { get; set; }
    }
}
