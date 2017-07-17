using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
using System.Drawing.Printing;
using System.Drawing;
using ServicePOS;
using ServicePOS.Model;


namespace Printer
{
   public class PrinterServer
    {
       Printer printer = new Printer();
       POSPrinter posPrinter = new POSPrinter();
       OrderDateModel OrderMain;
       MoneyFortmat money = new MoneyFortmat(MoneyFortmat.AU_TYPE);
       int Function;
       int PRINTBAR = 1;
       int PRINTBILL = 2;
       int pRINTReceipt = 3;
       OrderDateModel OrderPrint;
       ConfigModel cofig = new ServicePOS.Model.ConfigModel();
       string Header = string.Empty;

       private IPrinterService _printService;
       private IPrinterService PrintService
       {
           get { return _printService ?? (_printService = new PrinterService()); }
           set { _printService = value; }
       }

       private IConfigService _configService;
       private IConfigService ConfigService
       {
           get { return _configService ?? (_configService = new ConfigService()); }
           set { _configService = value; }
       }

       public PrinterServer()
       {
           var list = ConfigService.GetConfig();
           foreach (ConfigModel item in list)
           {
               cofig.ABN = item.ABN;
               cofig.Name = item.Name;
               cofig.Tel = item.Tel;
               cofig.Web = item.Web;
               cofig.Logan = item.Logan;
               cofig.Note = item.Note;
               cofig.Address = item.Address;
           }
       }
       private void GetListPrinter()
       { 
                   
       }
       
       private OrderDateModel SliptPriter(OrderDateModel OrderS,List<PrinterModel> printData)
       {
           OrderDateModel OrderSlipt = new OrderDateModel();
           for(int i = 0; i< printData.Count; i++)
           {
               for (int j = 0; j < OrderS.ListOrderDetail.Count; j++)
               {
                   for (int k = 0; k < OrderS.ListOrderDetail[j].ListPrintJob.Count; k++)
                   {
                       if (OrderS.ListOrderDetail[j].ListPrintJob[k].PrinterID == printData[i].ID)
                       {
                           OrderSlipt.ListOrderDetail.Add(OrderS.ListOrderDetail[k]);
                       }
                   }
               }
           }
           return OrderS;
       }
       public void PrintData(OrderDateModel _OrderMain,List<PrinterModel> printData)
       {
           OrderMain = _OrderMain;
          
           if (OrderMain.PrintType == this.PRINTBAR)
           {
               for (int i = 0; i < printData.Count; i++)
               {
                   OrderPrint = new OrderDateModel();
                  
                   for (int j = 0; j < OrderMain.ListOrderDetail.Count; j++)
                   {
                       for (int k = 0; k < OrderMain.ListOrderDetail[j].ListPrintJob.Count; k++)
                       {
                           if (OrderMain.IsLoadFromData && (OrderMain.ListOrderDetail[j].ChangeStatus == 2 || OrderMain.ListOrderDetail[j].ChangeStatus == 1 || OrderMain.ListOrderDetail[j].ChangeStatus == 3))
                           {
                               if (OrderMain.ListOrderDetail[j].ListPrintJob[k].PrinterID == printData[i].ID )
                               {
                                   if (OrderMain.ListOrderDetail[j].ChangeStatus == 2 || OrderMain.ListOrderDetail[j].ChangeStatus == 1 || OrderMain.ListOrderDetail[j].ChangeStatus == 3)
                                        OrderPrint.ListOrderDetail.Add(OrderMain.ListOrderDetail[j]);
                               }
                           }
                           else
                           {
                                   if (OrderMain.ListOrderDetail[j].ListPrintJob[k].PrinterID == printData[i].ID)
                                   {
                                       OrderPrint.ListOrderDetail.Add(OrderMain.ListOrderDetail[j]);
                                   }
                                  
                               
                           }
                       }
                       
                   }
                   
                    
                   if (OrderPrint.ListOrderDetail.Count > 0|| OrderMain.HaveNote>0)
                   {
                       
                       if (printData[i].PrinterType == 1)
                       {
                           Header = printData[i].Header;
                           if (OrderMain.PrinterNote == printData[i].ID)
                           {
                               OrderPrint.Note = OrderMain.Note;
                               OrderPrint.HaveNote = OrderMain.HaveNote;
                           }
                           Print(OrderPrint, printData[i]);
                       }
                       
                   }
                   if (OrderMain.ListOrderDetail.Count > 0)
                   {
                       if (printData[i].PrinterType == 0)
                       {
                           Header = printData[i].Header;
                           Print(OrderMain, printData[i]);
                       }
                   }
               }
               
           }
           else
           {
               if (OrderMain.PrintType == this.PRINTBILL)
               {
                   if (printData.Count > 0)
                       Print(OrderMain, printData[0]);
               }
               else
               {
                   if (OrderMain.PrintType == this.pRINTReceipt)
                   {
                       if (printData.Count > 0)
                           Print(OrderMain, printData[0]);
                   }
               }
           }
       }
       public void Print(OrderDateModel _OrderMain,PrinterModel data)
       {    
           posPrinter.SetPrinterName(data.PrinterName);
           posPrinter.printDocument.PrintPage += printDocument_PrintPage;
           if (OrderMain.IsLoadFromData && OrderMain.PrintType!=this.PRINTBILL)
           {
               if (OrderMain.IsPrePrint)
               {
                   posPrinter.Print();
               }
               else
               {
                   if (checkOrderChange(_OrderMain))
                   {
                       posPrinter.Print();
                   }
                   if (checkOrderChangeRemove(_OrderMain))
                   {
                       posPrinter.Print();
                   }
                   if(_OrderMain.PrintType==this.pRINTReceipt)
                   {
                       posPrinter.Print();
                   }
                   
               }
           }
           else
           {
               posPrinter.Print();
           }
           
       }
       void printDocument_PrintPage(object sender, PrintPageEventArgs e)
       {

           if (OrderMain.PrintType == this.PRINTBAR)
           {
               if(OrderPrint.ListOrderDetail.Count>0)
                   PrintOrderToKitchenOrBar(OrderPrint, e);
               else
                   PrintOrderToKitchenOrBar(OrderMain, e);
           }
           else
           {
               if (OrderMain.PrintType == this.PRINTBILL)
               {
                   PrintBill(e);
               }
               else
               {
                   if (OrderMain.PrintType == this.pRINTReceipt)
                       ButtonPrintBill(e);
               }

           }
              
       }
       public bool checkOrderChange(OrderDateModel order)
       {
           bool flag = false;
           for (int i = 0; i < order.ListOrderDetail.Count; i++)
           {
               if (order.ListOrderDetail[i].ChangeStatus == 1)
                   flag = true;
               if (order.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
               {
                   for (int j = 0; j < order.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                   {
                       if (order.ListOrderDetail[i].ListOrderDetailModifire[j].ChangeStatus == 1)
                           flag = true;
                   }
               }

           }
           if (order.HaveNote == 1)
               flag = true;
           
           return flag;
       }
       public bool checkOrderChangeRemove(OrderDateModel order)
       {
           bool flag = false;
           for (int i = 0; i < order.ListOrderDetail.Count; i++)
           {
               if (order.ListOrderDetail[i].ChangeStatus == 2)
                   flag= true;
               if (order.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
               {
                   for (int j = 0; j < order.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                   {
                       if (order.ListOrderDetail[i].ListOrderDetailModifire[j].ChangeStatus==2)
                       flag = true;
                   }
               }
              
           }
           return flag;
       }
       public float PrintOrderToKitchenOrBar(OrderDateModel Order, PrintPageEventArgs e)
       {
           float l_y = 0;
           if (OrderMain.IsLoadFromData)
           {
               if (OrderMain.IsPrePrint)
               {
                  
                   l_y = posPrinter.DrawString(Header, e, new Font("Arial", 14, FontStyle.Italic), l_y, 2,false);
                   l_y += posPrinter.GetHeightPrinterLine() / 10;
                   l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
                   l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                   l_y += posPrinter.GetHeightPrinterLine() / 10;
                   l_y = posPrinter.DrawString("OPERATOR#"+ OrderMain.UserName, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                   posPrinter.DrawString("Table# " + OrderMain.FloorID, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                   l_y = posPrinter.DrawString("Order#" + OrderMain.OrderID, e, new Font("Arial", 14), l_y, 3,false);
                   posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
                   l_y += posPrinter.GetHeightPrinterLine() / 10;
                   int countItem = 0;
                   for (int i = 0; i < Order.ListOrderDetail.Count; i++)
                   {
                       

                               countItem++;
                               l_y = posPrinter.DrawString(Order.ListOrderDetail[i].Qty.ToString() + " " + Order.ListOrderDetail[i].ProductName, e, new Font("Arial", 14), l_y, 1,false);
                        

                       if (Order.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
                       {
                           for (int j = 0; j < Order.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                           {
                               
                                       l_y = posPrinter.DrawString("--" + Order.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireName, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);

                                   
                           }
                       }
                   }
                   l_y += posPrinter.GetHeightPrinterLine() / 10;

                   posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
                   l_y = posPrinter.DrawString("", e, new Font("Arial", 14), l_y, 1,false);
                   l_y += posPrinter.GetHeightPrinterLine() / 10;
                   posPrinter.DrawString("Total item: " + countItem, e, new Font("Arial", 14, FontStyle.Bold), l_y, 1,false);
                   l_y += posPrinter.GetHeightPrinterLine() / 2;
                   if (Order.Note != null)
                       posPrinter.DrawString("Note:" + OrderMain.Note, e, new Font("Arial", 14, FontStyle.Bold), l_y, 1,false);
               }
               else
               {
                   if (checkOrderChange(Order))
                   {
                       l_y = posPrinter.DrawString(Header, e, new Font("Arial", 14, FontStyle.Italic), l_y, 2,false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
                       l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       l_y = posPrinter.DrawString("OPERATOR#"+ OrderMain.UserName, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                       posPrinter.DrawString("Table# " + OrderMain.FloorID, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                       l_y = posPrinter.DrawString("Order#" + OrderMain.OrderID, e, new Font("Arial", 14), l_y, 3,false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
                       posPrinter.DrawString("ADD", e, new Font("Arial", 14, FontStyle.Italic), l_y, 2,false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       for (int i = 0; i < Order.ListOrderDetail.Count; i++)
                       {
                           if (Order.ListOrderDetail[i].ChangeStatus == 1)
                           {

                               l_y = posPrinter.DrawString(Order.ListOrderDetail[i].ProductName, e, new Font("Arial", 14), l_y, 1,false);
                           }
                           if (Order.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
                           {
                               for (int j = 0; j < Order.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                               {
                                   if (Order.ListOrderDetail[i].ListOrderDetailModifire[j].ChangeStatus == 1)
                                   {

                                       l_y = posPrinter.DrawString(Order.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireName, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
                                   }
                               }
                           }
                       }
                       if (Order.HaveNote == 1)
                       {
                           posPrinter.DrawString("Note:" + OrderMain.Note, e, new Font("Arial", 14, FontStyle.Bold), l_y, 1,false);
                           
                       }
                   }
                   if (checkOrderChangeRemove(Order))
                   {
                       l_y = posPrinter.DrawString(Header, e, new Font("Arial", 14, FontStyle.Italic), l_y, 2,false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
                       l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       l_y = posPrinter.DrawString("OPERATOR#"+ OrderMain.UserName, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                       posPrinter.DrawString("Table# " + OrderMain.FloorID, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
                       l_y = posPrinter.DrawString("Order#" + OrderMain.OrderID, e, new Font("Arial", 14), l_y, 3,false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
                       posPrinter.DrawString("REMOVE", e, new Font("Arial", 14, FontStyle.Italic), l_y, 2, false);
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       l_y += posPrinter.GetHeightPrinterLine() / 10;
                       for (int i = 0; i < Order.ListOrderDetail.Count; i++)
                       {
                           float yStart = l_y;
                           if (Order.ListOrderDetail[i].ChangeStatus == 2)
                           {
                               l_y = posPrinter.DrawString(Order.ListOrderDetail[i].ProductName, e, new Font("Arial", 14), l_y, 1,false);
                               posPrinter.DrawCancelLine(e, yStart, l_y);
                           }
                           if (Order.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
                           {
                               for (int j = 0; j < Order.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                               {
                                   if (Order.ListOrderDetail[i].ListOrderDetailModifire[j].ChangeStatus == 2)
                                   {
                                       l_y = posPrinter.DrawString(Order.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireName, e, new Font("Arial", 10), l_y, 1,false);
                                       posPrinter.DrawCancelLine(e, yStart, l_y);
                                   }
                               }
                           }
                       }
                   }
               }
           }
           else
           {
               if (OrderMain.isTKA == 1)
               {
                   l_y = posPrinter.DrawString("Takeaway", e, new Font("Arial", 14, FontStyle.Italic), l_y, 2,false);
               }
                   
               l_y = posPrinter.DrawString(Header, e, new Font("Arial", 14, FontStyle.Italic), l_y, 2,false);
               l_y += posPrinter.GetHeightPrinterLine() / 10;
               l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
               l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
               l_y += posPrinter.GetHeightPrinterLine() / 10;
               l_y = posPrinter.DrawString("OPERATOR#"+ OrderMain.UserName, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
               posPrinter.DrawString("Table# " + OrderMain.FloorID, e, new Font("Arial", 14, FontStyle.Italic), l_y, 1,false);
               l_y = posPrinter.DrawString("Order#" + OrderMain.OrderID, e, new Font("Arial", 14), l_y, 3,false);
               posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
               l_y += posPrinter.GetHeightPrinterLine() / 10;
               int countItem = 0;
               for (int i = 0; i < Order.ListOrderDetail.Count; i++)
               {
                     
                           countItem++;
                           l_y = posPrinter.DrawString(Order.ListOrderDetail[i].Qty.ToString() + " " + Order.ListOrderDetail[i].ProductName, e, new Font("Arial", 14), l_y, 1,false);
                       
                   

                   if (Order.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
                   {
                       for (int j = 0; j < Order.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                       {
                           

                                   
                                   l_y = posPrinter.DrawString("--" + Order.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireName, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
                           
                       }
                   }
               }
               l_y += posPrinter.GetHeightPrinterLine() / 10;

               posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
               l_y = posPrinter.DrawString("", e, new Font("Arial", 14), l_y, 1,false);
               l_y += posPrinter.GetHeightPrinterLine() / 10;
               posPrinter.DrawString("Total item: " + countItem, e, new Font("Arial", 14, FontStyle.Bold), l_y, 1,false);
               
               l_y += posPrinter.GetHeightPrinterLine() / 2;
               if (Order.Note != null)
                   posPrinter.DrawString("Note:" + OrderMain.Note, e, new Font("Arial", 14, FontStyle.Bold), l_y, 1,false);
           }
          
           return l_y;
       }

       private void ButtonPrintBill(PrintPageEventArgs e)
       {
           float l_y = 0;
           l_y = posPrinter.DrawString(cofig.Name, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2,false);
           l_y = posPrinter.DrawString(cofig.ABN, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2,false);
           l_y = posPrinter.DrawString(cofig.Address, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2,false);
           l_y = posPrinter.DrawString(cofig.Tel, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2,false);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
           l_y = posPrinter.DrawString("Table# " + OrderMain.FloorID, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
           l_y = posPrinter.DrawString("OPERATOR#"+ OrderMain.UserName, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           l_y = posPrinter.DrawString("BILL", e, new Font("Arial", 10, FontStyle.Bold), l_y, 2,false);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           for (int i = 0; i < OrderMain.ListOrderDetail.Count; i++)
           {
               float yStart = l_y;
               posPrinter.DrawString(OrderMain.ListOrderDetail[i].ProductName, e, new Font("Arial", 10, FontStyle.Bold), l_y, 1,true);
               l_y = posPrinter.DrawString(OrderMain.ListOrderDetail[i].Qty.ToString(), e, new Font("Arial", 10, FontStyle.Bold), l_y, 2,false);
               posPrinter.DrawString("$" + money.Format2(OrderMain.ListOrderDetail[i].Price.ToString()), e, new Font("Arial", 10, FontStyle.Bold), yStart, 3,false);
               if (OrderMain.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
               {
                   for (int j = 0; j < OrderMain.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                   {
                       l_y = posPrinter.DrawString("__" + OrderMain.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireName, e, new Font("Arial", 10), l_y, 1,false);
                   }
               }
               if (e.Graphics.MeasureString(OrderMain.ListOrderDetail[i].ProductName, new Font("Arial", 10, FontStyle.Bold)).Width > (float)Math.Abs(e.PageBounds.Width - 173))
               {
                   l_y += posPrinter.GetHeightPrinterLine() / 6;
               }
           }
           
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           posPrinter.DrawString("SubTotal:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1,false);
           l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.TotalAmount)), e, new Font("Arial", 10), l_y, 3,false);
           //if (OrderMain.Discount > 0)
           //{
           //    posPrinter.DrawString("Discount:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1);
           //    l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.Discount), e, new Font("Arial", 10), l_y, 3);

           //    posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1);
           //    l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.SubTotal() - OrderMain.Discount), e, new Font("Arial", 10), l_y, 3);
           //}
           //else
           //{
           //    posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1);
           //    l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.SubTotal()), e, new Font("Arial", 10), l_y, 3);
           //}
           //posPrinter.DrawString("GST(Include in total):", e, new Font("Arial", 10), l_y, 1);
           //l_y = posPrinter.DrawString("$1.0", e, new Font("Arial", 10), l_y, 3);
           //if (OrderMain.ListPayment.Count > 0)
           //{
           //    for (int j = 0; j < OrderMain.ListPayment.Count; j++)
           //    {
           //        if (OrderMain.ListPayment[j].PaymentTypeID == 1)
           //        {
           //            posPrinter.DrawString("Cash:", e, new Font("Arial", 10), l_y, 1);
           //            l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total * 1000), e, new Font("Arial", 10), l_y, 3);
           //        }
           //        if (OrderMain.ListPayment[j].PaymentTypeID == 2)
           //        {
           //            posPrinter.DrawString("Card:", e, new Font("Arial", 10), l_y, 1);
           //            l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total * 1000), e, new Font("Arial", 10), l_y, 3);
           //            if (OrderMain.ListInvoiceByCard.Count > 0)
           //            {
           //                for (int i = 0; i < OrderMain.ListInvoiceByCard.Count; i++)
           //                {
           //                    posPrinter.DrawString("--" + OrderMain.ListInvoiceByCard[i].NameCard, e, new Font("Arial", 10), l_y, 1);
           //                    l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.ListInvoiceByCard[i].Total)), e, new Font("Arial", 10), l_y, 3);
           //                }
           //            }
           //        }
           //    }
           //}
           //if (OrderMain.Change > 0)
           //{
           //    posPrinter.DrawString("Change:", e, new Font("Arial", 10), l_y, 1);
           //    l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.Change), e, new Font("Arial", 10), l_y, 3);

           //}
           //l_y += posPrinter.GetHeightPrinterLine() / 2;
           //l_y = posPrinter.DrawString(cofig.Web, e, new Font("Arial", 10), l_y, 2);
           //l_y = posPrinter.DrawString(cofig.Logan, e, new Font("Arial", 10), l_y, 2);
           //l_y = posPrinter.DrawString(cofig.Note, e, new Font("Arial", 10), l_y, 2);
       }

       private void PrintReceipt(PrintPageEventArgs e)
       {
           float l_y = 0;
           l_y = posPrinter.DrawString(cofig.Name, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.ABN, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Address, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Tel, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2, false);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 10, FontStyle.Italic), l_y, 1, false);
           l_y = posPrinter.DrawString("Tranx #", e, new Font("Arial", 10, FontStyle.Italic), l_y, 1, false);
           l_y = posPrinter.DrawString("Table# " + OrderMain.FloorID, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1, false);
           l_y = posPrinter.DrawString("OPERATOR#" + OrderMain.UserName, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1, false);
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           l_y = posPrinter.DrawString("BILL", e, new Font("Arial", 10, FontStyle.Bold), l_y, 2, false);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           for (int i = 0; i < OrderMain.ListOrderDetail.Count; i++)
           {
               float yStart = l_y;
               posPrinter.DrawString(OrderMain.ListOrderDetail[i].ProductName, e, new Font("Arial", 10, FontStyle.Bold), l_y, 1, false);
               l_y = posPrinter.DrawString(OrderMain.ListOrderDetail[i].Qty.ToString(), e, new Font("Arial", 10, FontStyle.Bold), l_y, 2, false);
               posPrinter.DrawString("$" + money.Format2(OrderMain.ListOrderDetail[i].Price.ToString()), e, new Font("Arial", 10, FontStyle.Bold), yStart, 3, false);
               if (OrderMain.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
               {
                   for (int j = 0; j < OrderMain.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                   {
                       l_y = posPrinter.DrawString("__" + OrderMain.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireName, e, new Font("Arial", 10), l_y, 1, false);
                   }
               }
           }
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           posPrinter.DrawString("SubTotal:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1, false);
           l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.TotalAmount)), e, new Font("Arial", 10), l_y, 3, false);
           if (OrderMain.Discount > 0)
           {
               posPrinter.DrawString("Discount:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1, false);
               l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.Discount), e, new Font("Arial", 10), l_y, 3, false);

               posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1, false);
               l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.SubTotal() - OrderMain.Discount), e, new Font("Arial", 10), l_y, 3, false);
           }
           else
           {
               posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1, false);
               l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.SubTotal()), e, new Font("Arial", 10), l_y, 3, false);
           }
           posPrinter.DrawString("GST(Include in total):", e, new Font("Arial", 10), l_y, 1, false);
           l_y = posPrinter.DrawString("$1.0", e, new Font("Arial", 10), l_y, 3, false);
           if (OrderMain.ListPayment.Count > 0)
           {
               for (int j = 0; j < OrderMain.ListPayment.Count; j++)
               {
                   if (OrderMain.ListPayment[j].PaymentTypeID == 1)
                   {
                       posPrinter.DrawString("Cash:", e, new Font("Arial", 10), l_y, 1, false);
                       l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total * 1000), e, new Font("Arial", 10), l_y, 3, false);
                   }
                   if (OrderMain.ListPayment[j].PaymentTypeID == 2)
                   {
                       posPrinter.DrawString("Card:", e, new Font("Arial", 10), l_y, 1, false);
                       l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total * 1000), e, new Font("Arial", 10), l_y, 3, false);
                        if (OrderMain.ListInvoiceByCard.Count > 0)
                        {
                            for (int i = 0; i < OrderMain.ListInvoiceByCard.Count; i++)
                            {
                                posPrinter.DrawString("--" + OrderMain.ListInvoiceByCard[i].NameCard, e, new Font("Arial", 10), l_y, 1, false);
                                l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.ListInvoiceByCard[i].Total)), e, new Font("Arial", 10), l_y, 3, false);
                            }
                        }
                   }
               }
           }
           if (OrderMain.Change > 0)
           {
               posPrinter.DrawString("Change:", e, new Font("Arial", 10), l_y, 1, false);
               l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.Change), e, new Font("Arial", 10), l_y, 3, false);
 
           }
           l_y += posPrinter.GetHeightPrinterLine() / 2;
           l_y = posPrinter.DrawString(cofig.Web, e, new Font("Arial", 10), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Logan, e, new Font("Arial", 10), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Note, e, new Font("Arial", 10), l_y, 2, false);

       }

       //FUNCTION PRINT BILL
       private void PrintBill(PrintPageEventArgs e)
       {
           StaffModel staff = new ServicePOS.Model.StaffModel();
           
           float l_y = 0;
           l_y = posPrinter.DrawString(cofig.Name, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.ABN, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2,false);
           l_y = posPrinter.DrawString(cofig.Address, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2,false);
           l_y = posPrinter.DrawString(cofig.Tel, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2,false);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
           
           l_y = posPrinter.DrawString("Table# " + OrderMain.FloorID, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
           l_y = posPrinter.DrawString("OPERATOR#"+OrderMain.UserName , e, new Font("Arial", 10, FontStyle.Italic), l_y, 1,false);
           
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           l_y = posPrinter.DrawString(" TAX INVOICE", e, new Font("Arial", 10, FontStyle.Bold), l_y, 2,false);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           for (int i = 0; i < OrderMain.ListOrderDetail.Count; i++)
           {
               float yStart = l_y;
               posPrinter.DrawString(OrderMain.ListOrderDetail[i].ProductName, e, new Font("Arial", 10, FontStyle.Bold), l_y, 1,false);
               l_y = posPrinter.DrawString(OrderMain.ListOrderDetail[i].Qty.ToString(), e, new Font("Arial", 10, FontStyle.Bold), l_y, 2,false);
               posPrinter.DrawString("$" + money.Format2(OrderMain.ListOrderDetail[i].Price.ToString()), e, new Font("Arial", 10, FontStyle.Bold), yStart, 3,false);
               if (OrderMain.ListOrderDetail[i].ListOrderDetailModifire.Count > 0)
               {
                   for (int j = 0; j < OrderMain.ListOrderDetail[i].ListOrderDetailModifire.Count; j++)
                   {
                       l_y = posPrinter.DrawString("__" + OrderMain.ListOrderDetail[i].ListOrderDetailModifire[j].ModifireName, e, new Font("Arial", 10), l_y, 1,false);
                   }
               }
           }
           l_y += posPrinter.GetHeightPrinterLine() / 10;

           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1,false);
           l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.TotalAmount)), e, new Font("Arial", 10), l_y, 3,false);

           posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           posPrinter.DrawString("G.S.T Content:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1,false);
           Double gst = Convert.ToDouble(OrderMain.TotalAmount) / 11000;
           if(gst<1)
                l_y = posPrinter.DrawString("$0" + money.Format2(Convert.ToDouble(OrderMain.TotalAmount)/11), e, new Font("Arial", 10), l_y, 3,false);
           else
               l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.TotalAmount) / 11), e, new Font("Arial", 10), l_y, 3,false);
           //posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           //posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1);
           //l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.TotalAmount)), e, new Font("Arial", 10), l_y, 3);
           

           if (OrderMain.Discount > 0)
           {
               posPrinter.DrawString("Discount:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1,false);
               l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.Discount), e, new Font("Arial", 10), l_y, 3,false);

               posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1,false);
               l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.SubTotal() - OrderMain.Discount), e, new Font("Arial", 10), l_y, 3,false);
           }
           else
           {
               //posPrinter.DrawString("Total:", e, new Font("Arial", 10, FontStyle.Bold), l_y, 1);
               //l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.SubTotal()), e, new Font("Arial", 10), l_y, 3);
           }
           //posPrinter.DrawString("GST(Include in total):", e, new Font("Arial", 10), l_y, 1);
           //l_y = posPrinter.DrawString("$1.0", e, new Font("Arial", 10), l_y, 3);
           if (OrderMain.ListPayment.Count > 0)
           {
               for (int j = 0; j < OrderMain.ListPayment.Count; j++)
               {
                   if (OrderMain.ListPayment[j].PaymentTypeID == 1)
                   {
                       posPrinter.DrawString("Cash:", e, new Font("Arial", 10), l_y, 1,false);
                       if(OrderMain.isPrevOrder==1)
                            l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total), e, new Font("Arial", 10), l_y, 3,false);
                       else
                           l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total*1000), e, new Font("Arial", 10), l_y, 3,false);
                   }
                   if (OrderMain.ListPayment[j].PaymentTypeID == 2)
                   {
                       posPrinter.DrawString("Card:", e, new Font("Arial", 10), l_y, 1,false);
                        if(OrderMain.isPrevOrder==1)
                           l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total), e, new Font("Arial", 10), l_y, 3,false);
                        else
                            l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.ListPayment[j].Total*1000), e, new Font("Arial", 10), l_y, 3,false);
                       if (OrderMain.ListInvoiceByCard.Count > 0)
                       {
                           for (int i = 0; i < OrderMain.ListInvoiceByCard.Count; i++)
                           {
                               posPrinter.DrawString("--" + OrderMain.ListInvoiceByCard[i].NameCard, e, new Font("Arial", 10), l_y, 1,false);
                               l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.ListInvoiceByCard[i].Total)), e, new Font("Arial", 10), l_y, 3,false);
                           }
                       }
                   }
               }
           }
           if (OrderMain.Change > 0)
           {
               posPrinter.DrawString("Change:", e, new Font("Arial", 10), l_y, 1,false);
               l_y = posPrinter.DrawString("$" + money.Format2(OrderMain.Change), e, new Font("Arial", 10), l_y, 3,false);

           }
           if (OrderMain.CusItem.ClientID>0)
           {
               posPrinter.DrawString("Account:", e, new Font("Arial", 10), l_y, 1,false);
               l_y = posPrinter.DrawString("$" + money.Format2(Convert.ToDouble(OrderMain.SubTotal())), e, new Font("Arial", 10), l_y, 3,false);
               posPrinter.DrawString("AccountName:", e, new Font("Arial", 10), l_y, 1,false);
               l_y = posPrinter.DrawString(OrderMain.CusItem.Fname, e, new Font("Arial", 10), l_y, 3,false);
           }
           l_y += posPrinter.GetHeightPrinterLine() / 2;
           l_y = posPrinter.DrawString(cofig.Web, e, new Font("Arial", 10), l_y, 2,false);
           l_y = posPrinter.DrawString(cofig.Logan, e, new Font("Arial", 10), l_y, 2,false);
           l_y = posPrinter.DrawString(cofig.Note, e, new Font("Arial", 10), l_y, 2,false);

       }
       public void printJoinTable(string JoinTable, PrintPageEventArgs e)
       {
           float l_y = 0;
           l_y = posPrinter.DrawString(cofig.Name, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.ABN, e, new Font("Arial", 10, FontStyle.Bold), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Address, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Tel, e, new Font("Arial", 10, FontStyle.Italic), l_y, 2, false);
           l_y += posPrinter.GetHeightPrinterLine() / 10;
           l_y = posPrinter.DrawLine("", new Font("Arial", 14), e, System.Drawing.Drawing2D.DashStyle.Dot, l_y, 1);
           l_y = posPrinter.DrawString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), e, new Font("Arial", 10, FontStyle.Italic), l_y, 1, false);
           l_y = posPrinter.DrawString(JoinTable, e, new Font("Arial", 10, FontStyle.Italic), l_y, 1, false);
           l_y += posPrinter.GetHeightPrinterLine() / 2;
           l_y = posPrinter.DrawString(cofig.Web, e, new Font("Arial", 10), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Logan, e, new Font("Arial", 10), l_y, 2, false);
           l_y = posPrinter.DrawString(cofig.Note, e, new Font("Arial", 10), l_y, 2, false);

       }
    }
}
  