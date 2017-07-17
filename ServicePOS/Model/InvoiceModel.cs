using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class InvoiceModel
    {
        public int InvoiceID { get; set; }
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string FloorID { get; set; }
        public int Status { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Note { get; set; }
        public int Seat { get; set; }
        public int ShiftID { get; set; }
        public bool IsLoadFromData { get; set; }
        public List<InvoiceDetailModel> ListOrderDetail = new List<InvoiceDetailModel>();
        public List<CardModel> ListCard = new List<CardModel>();
        public int InvoiceNumber { get; set; }
        public int Account { get; set; }
    }
}
