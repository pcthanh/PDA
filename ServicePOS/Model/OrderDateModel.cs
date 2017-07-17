using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
   public class OrderDateModel
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
        public bool IsLoadFromData { get; set; }
        public bool IsPrePrint { get; set; }
        public int DiscountType { get; set; }
        public int Discount { get; set; }
        public int Payment { get; set; }
        public int Change { get; set; }
        public double CashOut { get; set; }
        public int isTKA { get; set; }
        public int isNoPrintBill { get; set; }
        public int ShiftID { get; set; }
        public int isTransferTableAll { get; set; }
        public int isPrevOrder { get; set; }
        public int PrintType { get; set; }
        public string InvoiceNumber { get; set; }
        public int Account { get; set; }
        public int PrinterNote { get; set; }
        public int HaveNote { get; set; }
        public string UserName { get; set; }
        public string SendOrderOrNot { get; set; }
        public List<OrderDetailModel> ListOrderDetail = new List<OrderDetailModel>();
        public List<CardModel> ListCard = new List<CardModel>();
        public List<CashModel> ListCash = new List<CashModel>();
        public List<PayMentModel> ListPayment = new List<PayMentModel>();
        public List<InvoiceByCardModel> ListInvoiceByCard = new List<InvoiceByCardModel>();
        public List<SeatModel> ListSeatOfOrder = new List<SeatModel>();
        public CustomerModel CusItem = new CustomerModel();
        public int PDA { get; set; }
        private IOrderService _orderService;
        private IOrderService OrderService
        {
            get { return _orderService ?? (_orderService = new OrderService()); }
            set { _orderService = value; }
        }
        
        public void addItemToList(OrderDetailModel item)
        {
            item.KeyItem = ListOrderDetail.Count + 1;
            item.Total = item.Price * item.Qty;
            ListOrderDetail.Add(item);
        }
        public void addItemToListAddSeat(OrderDetailModel item)
        {
            
            item.Total = item.Price * item.Qty;
            ListOrderDetail.Add(item);
        }
        public void addModifierToList(OrderDetailModifireModel modifire, int keyItem)
        {
            if (ListOrderDetail.Count > 0)
            {
                
                if (modifire.ChangeStatus == 1 &&ListOrderDetail[keyItem-1].ChangeStatus==0)
                {
                    ListOrderDetail[keyItem - 1].ChangeStatus = 3;
                }
                modifire.KeyItem = ListOrderDetail[keyItem - 1].ListOrderDetailModifire.Count + 1;
                modifire.KeyModi = ListOrderDetail[keyItem - 1].KeyItem;
                modifire.ProductID = ListOrderDetail[keyItem - 1].ProductID;
                modifire.Seat = ListOrderDetail[keyItem - 1].Seat;
                modifire.Total = ListOrderDetail[keyItem - 1].Price * ListOrderDetail[keyItem - 1].Qty;
                
                ListOrderDetail[keyItem - 1].ListOrderDetailModifire.Add(modifire);

            }
        }
        public Double SubTotal()
        {
            Double total = 0;
            for (int i = 0; i < ListOrderDetail.Count; i++)
            {

                total += Convert.ToDouble(ListOrderDetail[i].Price * ListOrderDetail[i].Qty);
                if (ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
                {
                    for (int j = 0; j < ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                    {
                        total += Convert.ToDouble(ListOrderDetail[i].ListOrderDetailModifire[j].Price * ListOrderDetail[i].ListOrderDetailModifire[j].Qty);
                    }
                }
            }
            
            TotalAmount = total;
            return total;
        }
        public Double SubTotalVoid()
        {
            Double total = 0;
            for (int i = 0; i < ListOrderDetail.Count; i++)
            {
                if(ListOrderDetail[i].ChangeStatus!=2)
                {
                    total += Convert.ToDouble(ListOrderDetail[i].Price * ListOrderDetail[i].Qty);
                    if (ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
                    {
                        for (int j = 0; j < ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                        {
                            total += Convert.ToDouble(ListOrderDetail[i].ListOrderDetailModifire[j].Price * ListOrderDetail[i].ListOrderDetailModifire[j].Qty);
                        }
                    }
                }
            }

            TotalAmount = total;
            return total;
        }
        public void addSeat(int numberSeat)
        {
            Seat = numberSeat;
        }
        public int CountIteminSeat(int numSeat)
        {
            int count = 0;
            for (int i = 0; i < ListOrderDetail.Count; i++)
            {
                
                if (ListOrderDetail[i].Seat == numSeat)
                {
                    count++;
                    count =count+ ListOrderDetail[i].ListOrderDetailModifire.Count;
                }
            }
            return count;
        }
    }
}
