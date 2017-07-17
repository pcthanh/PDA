using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class PriceListModel
    {
        public int ID { get; set; }
        public string NameDesc { get; set; }
        public string NameSort { get; set; }
        public string Color { get; set; }
        public Nullable<double> CurrentPrice { get; set; }
        public string Type { get; set; }
        public string Portions { get; set; }
        public int PriceID { get; set; }
    }
}
