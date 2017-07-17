using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelPOS.ModelEntity;
using ServicePOS.Model;
using SystemLog;
namespace ServicePOS
{
   public class InvoiceService:IInvoiceService
    {
        private POSEZ2UEntities _context;
        public InvoiceService()
        {
            _context = new POSEZ2UEntities();
        }
        public InvoiceService(POSEZ2UEntities context)
        {
            _context = context;
        }
        

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // code is here
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public int CountInvoie()
        {
            return _context.INVOICEs.Count();
        }

        public int InsertInvoice(OrderDateModel Order)
        {
            int result = 0;
            int InvoiceID;
            INVOICE invoice = new INVOICE();
            List<INVOICE_DETAIL> invoiceDEtail = new List<INVOICE_DETAIL>();
            List<INVOICE_DETAIL_MODIFIRE> invoiceDetailModifier = new List<INVOICE_DETAIL_MODIFIRE>();
            List<PAYMENT_INVOICE_HISTORY> PaymentHistory = new List<PAYMENT_INVOICE_HISTORY>();
            List<INVOICE_BY_CARD> InvoiceByCard = new List<INVOICE_BY_CARD>();
            ACC_PAYMENT Acc = new ACC_PAYMENT();
            using (var trans = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlCommand("update ORDER_DATE set Status=1 where OrderID='" + Order.OrderID + "'");
                invoice = CopyInvoice(Order);
                _context.Entry(invoice).State = System.Data.Entity.EntityState.Added;
                _context.SaveChanges();
                InvoiceID = invoice.InvoiceID;
                string InvNum = InvoiceID + "" + DateTime.Now.Date.Year + "" + DateTime.Now.Date.Month + "" + DateTime.Now.Date.Day;
                _context.Database.ExecuteSqlCommand("update invoice set InvoiceNumber='" + InvNum + "'where InvoiceID='" + InvoiceID + "'");
                
                //Update balance for customer
                _context.Database.ExecuteSqlCommand("update client set balance=balance+'" + -Convert.ToInt32(Order.Payment) + "' where ClientID='" + Order.CusItem.ClientID + "'");
                Acc = CopyAcc(Order);
                Acc.InvoiceID = InvoiceID;
                Acc.InvoiceNumber =(InvNum);
                //_context.Entry(Acc).State = System.Data.Entity.EntityState.Added;
                //string sql = "insert into acc_payment(CusNo,SubTotal,InvoiceID,InvoiceNumber,Cash,Card,IsCredit,IsDebit,CreateDate,CreateBy,UpdateDate,UpdateBy)values(" +
                //    "'" + Order.CusItem.ClientID + "','" + Convert.ToInt32(Order.Payment) + "','" + InvoiceID + "','" + Convert.ToInt32(InvNum) + "',0,0,1,0,'" + DateTime.Now + "','" + Order.ShiftID + "','" + DateTime.Now + "','" + Order.ShiftID + "')";
                if(Order.CusItem.ClientID>0)
                     _context.Database.ExecuteSqlCommand("insert into acc_payment(CusNo,SubTotal,InvoiceID,InvoiceNumber,Cash,Card,IsCredit,IsDebit,CreateDate,CreateBy,UpdateDate,UpdateBy)values(" +
                    "'" + Order.CusItem.ClientID + "','" +Convert.ToInt32(Order.Payment) + "','" + InvoiceID + "','" +(InvNum) + "',0,0,1,0,'" + DateTime.Now + "','" + Order.ShiftID + "','" + DateTime.Now + "','" + Order.ShiftID + "')");
                //
                invoiceDEtail = CopyInvoicedetail(Order);
                foreach (INVOICE_DETAIL item in invoiceDEtail)
                {
                    item.InvoiceID = InvoiceID;
                    item.InvoiceNumber = (InvNum);
                    _context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }
                invoiceDetailModifier = CopyInvoiceMidifire(Order);

                foreach (INVOICE_DETAIL_MODIFIRE item in invoiceDetailModifier)
                {
                    item.InvoiceID = InvoiceID;
                    item.InvoiceNumber = (InvNum);
                   
                    _context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }

                PaymentHistory = CopyListPayment(Order);
                foreach (PAYMENT_INVOICE_HISTORY item in PaymentHistory)
                {
                    item.InvoiceID = InvoiceID;
                    item.InvoiceNumber = (InvNum);
                    _context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }

                InvoiceByCard = CopyInvoiceByCard(Order);
                foreach(INVOICE_BY_CARD item in InvoiceByCard)
                {
                    item.InvoiceID = InvoiceID;
                    _context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }

                _context.SaveChanges();
                trans.Commit();
                result = 1;
            }
            return result;
        }

        private ACC_PAYMENT CopyAcc(OrderDateModel Order)
        {
            ACC_PAYMENT acc = new ACC_PAYMENT();
            acc.SubTotal = Convert.ToInt32(Order.TotalAmount);
            acc.CusNo = Order.CusItem.ClientID;
            acc.InvoiceID = Order.InvoiceID;
            acc.InvoiceNumber = Order.InvoiceNumber;
            acc.Cash = 0;
            acc.Card = 0;
            acc.IsCredit = 1;
            acc.IsDebit = 0;
            acc.CreateBy = Order.CreateBy ?? 0;
            acc.CreateDate = DateTime.Now;
            acc.UpdateBy = Order.UpdateBy ?? 0;
            acc.UpdateDate = DateTime.Now;
            return acc;
            
        }
        private INVOICE CopyInvoice(OrderDateModel itemOrder)
        {
            INVOICE invoice = new INVOICE();
            invoice.InvoiceNumber = itemOrder.InvoiceNumber;
            invoice.OrderID = itemOrder.OrderID;
            invoice.OrderNumber =( itemOrder.OrderNumber);
            invoice.Total = Convert.ToInt32(itemOrder.TotalAmount);
            invoice.Status = 1;
            invoice.DiscountType = itemOrder.DiscountType;
            invoice.Discount =(itemOrder.Discount);
            invoice.Payment = itemOrder.Payment;
            invoice.Change = itemOrder.Change;
            invoice.CashOut =Convert.ToInt32(itemOrder.CashOut);
            if (itemOrder.ListInvoiceByCard.Count > 0)
                invoice.InvoiceByCardID =Convert.ToInt32(CustomerInvoiceByCardID());
            else
                invoice.InvoiceByCardID = 0;
            invoice.CreateBy = itemOrder.CreateBy ?? 0;
            invoice.CreateDate = DateTime.Now;
            invoice.UpdateBy = itemOrder.UpdateBy ?? 0;
            invoice.UpdateDate = DateTime.Now;
            invoice.Note = itemOrder.Note ?? "";
            invoice.ShiftID = itemOrder.ShiftID;
            if (itemOrder.FloorID.Contains("TKA-"))
            {
                invoice.FloorType = 0;
            }
            else
            {
                invoice.FloorType = 1;
            }
            
            return invoice;
        }
        private List<INVOICE_DETAIL> CopyInvoicedetail(OrderDateModel itemOrder)
        {
            List<INVOICE_DETAIL> lstorderDetailDate = new List<INVOICE_DETAIL>();
            lstorderDetailDate.Clear();
            try
            {

                for (int i = 0; i < itemOrder.ListOrderDetail.Count; i++)
                {
                    
                        INVOICE_DETAIL orderDetaiDate = new INVOICE_DETAIL();
                        
                        orderDetaiDate.OrderDetailID = itemOrder.ListOrderDetail[i].OrderDetailID;
                        orderDetaiDate.ProductID = itemOrder.ListOrderDetail[i].ProductID;
                        orderDetaiDate.DynId = itemOrder.ListOrderDetail[i].DynID;
                        orderDetaiDate.Seat = itemOrder.ListOrderDetail[i].Seat;
                        orderDetaiDate.Qty = itemOrder.ListOrderDetail[i].Qty;
                        orderDetaiDate.KeyItem = itemOrder.ListOrderDetail[i].KeyItem;
                        orderDetaiDate.Total = itemOrder.ListOrderDetail[i].Total;
                        orderDetaiDate.Price = itemOrder.ListOrderDetail[i].Price;
                        orderDetaiDate.Status = itemOrder.ListOrderDetail[i].Satust;
                        orderDetaiDate.Note = itemOrder.ListOrderDetail[i].Note;
                        orderDetaiDate.CreateBy = itemOrder.ListOrderDetail[i].CreateBy ?? 0;
                        orderDetaiDate.CreateDate = itemOrder.ListOrderDetail[i].CreateDate ?? DateTime.Now;
                        orderDetaiDate.UpdateBy = itemOrder.ListOrderDetail[i].UpdateBy ?? 0;
                        orderDetaiDate.UpdateDate = itemOrder.ListOrderDetail[i].UpdateDate ?? DateTime.Now;
                        lstorderDetailDate.Add(orderDetaiDate);
                    
                }
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("CopyOrderDetailDate:::::::::::::::::::::::::" + ex.Message);
            }
            return lstorderDetailDate;
        }
        private List<INVOICE_DETAIL_MODIFIRE> CopyInvoiceMidifire(OrderDateModel itemOrder)
        {
            List<INVOICE_DETAIL_MODIFIRE> lstOrderModifreDate = new List<INVOICE_DETAIL_MODIFIRE>();
            lstOrderModifreDate.Clear();
            try
            {
                for (int i = 0; i < itemOrder.ListOrderDetail.Count; i++)
                {
                    for (int j = 0; j < itemOrder.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                    {

                            INVOICE_DETAIL_MODIFIRE orderDetailModifire = new INVOICE_DETAIL_MODIFIRE();
                            orderDetailModifire.ModifireID = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireID;
                            
                            orderDetailModifire.OrderModifireID = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].OrderModifireID;
                            orderDetailModifire.ProductID = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].ProductID;
                            orderDetailModifire.KeyModi = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].KeyModi;    
                            orderDetailModifire.Price = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].Price;
                            orderDetailModifire.Qty = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].Qty;
                            orderDetailModifire.Total = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].Total;
                            orderDetailModifire.Status = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].Satust;
                            orderDetailModifire.Seat = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].Seat;
                            orderDetailModifire.DynId = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].DynID;
                            orderDetailModifire.CreateBy = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].CreateBy ?? 0;
                            orderDetailModifire.CreateDate = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].CreateDate ?? DateTime.Now;
                            orderDetailModifire.UpdateBy = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].UpdateBy ?? 0;
                            orderDetailModifire.UpdateDate = itemOrder.ListOrderDetail[i].ListOrderDetailModifire[j].UpdateDate ?? DateTime.Now;
                            lstOrderModifreDate.Add(orderDetailModifire);
                        
                    }

                }
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("CopyOrderMidifireDate:::::::::::::::::::::::::::::::::" + ex.Message);
            }
            return lstOrderModifreDate;
        }
        private List<PAYMENT_INVOICE_HISTORY> CopyListPayment(OrderDateModel Order)
        {
            List<PAYMENT_INVOICE_HISTORY> lst = new List<PAYMENT_INVOICE_HISTORY>();
            lst.Clear();
            for (int i = 0; i < Order.ListPayment.Count; i++)
            {
                PAYMENT_INVOICE_HISTORY item = new PAYMENT_INVOICE_HISTORY();
                item.PaymentTypeID = Order.ListPayment[i].PaymentTypeID;
                
                item.Status = 1;
                item.Total = Order.ListPayment[i].Total*1000;
                item.CreateBy = Order.ListPayment[i].CreateBy ?? 0;
                item.CreateDate = DateTime.Now;
                item.UpdateBy = Order.ListPayment[i].UpdateBy ?? 0;
                item.UpdateDate = DateTime.Now;
                item.Note = Order.ListPayment[i].Note ?? "";
                lst.Add(item);
            }
            return lst;
        }
        private List<INVOICE_BY_CARD> CopyInvoiceByCard(OrderDateModel Order)
        {
            List<INVOICE_BY_CARD> lst = new List<INVOICE_BY_CARD>();
            lst.Clear();
            for (int i = 0; i < Order.ListInvoiceByCard.Count; i++)
            {
                INVOICE_BY_CARD item = new INVOICE_BY_CARD();
                item.InvoiceByCardID =Convert.ToInt32(CustomerInvoiceByCardID());
                
                item.CardID = Order.ListInvoiceByCard[i].CardID;
                item.Total = Order.ListInvoiceByCard[i].Total;
                item.CreateBy = Order.ListInvoiceByCard[i].CreateBy ?? 0;
                item.CreateDate = DateTime.Now;
                item.UpdateBy = Order.ListInvoiceByCard[i].UpdateBy ?? 0;
                item.UpdateDate = DateTime.Now;
                item.Note = Order.ListInvoiceByCard[i].Note ?? "";
                lst.Add(item);
            }
            return lst;
        }
        private string CustomerInvoiceByCardID()
        {

            string ID = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + CountInvoie() + 1;
            return ID;
        }



        public int UpdateOrderInvice(OrderDateModel Order)
        {
            int result = 0;
            using (var trans = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlCommand("update ORDER_DATE set Status=1 where OrderID='" + (Order.OrderID-1) + "'");
                _context.SaveChanges();
                trans.Commit();
                result = 1;
            }
            return result;
        }
    }
}
