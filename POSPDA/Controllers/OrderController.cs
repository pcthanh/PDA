using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelPOS;
using ServicePOS;
using ServicePOS.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Printer;
namespace POSPDA.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        private OrderDateModel OrderMain = new OrderDateModel();
        List<PrinterModel> PrintData = new List<PrinterModel>();
        private string FloorID = Class.FloorID;
        private IOrderService _orderService;
        private IOrderService OrderService
        {
            get { return _orderService ?? (_orderService = new OrderService()); }
            set { _orderService = value; }
        }
        private IProductService _productService;
        private IProductService ProductService
        {
            get { return _productService ?? (_productService = new ProductService()); }
            set { _productService = value; }
        }
        private IPrinterService _printService;
        private IPrinterService PrintService
        {
            get { return _printService ?? (_printService = new PrinterService()); }
            set { _printService = value; }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendOrder(string lstOrderItem, string lstModifierItem, string lstSeat)
        {
            var session=Session["User"] as StaffModel;
            
            if (session != null)
            {
                GetListPrinter();
                try
                {
                    int KeyItem = 0;
                    string FloorID = Class.FloorID;
                    List<OrderItem> listOrderItem = new JavaScriptSerializer().Deserialize<List<OrderItem>>(lstOrderItem);
                    List<ModifierModel> listModifier = new JavaScriptSerializer().Deserialize<List<ModifierModel>>(lstModifierItem);
                    List<SeatModel> listSeat = new JavaScriptSerializer().Deserialize<List<SeatModel>>(lstSeat);
                    OrderDateModel OrderTemp = OrderService.GetOrderByTable(FloorID, 0);
                    if (OrderTemp.Status != 2)
                    {
                        if (OrderTemp.ListOrderDetail.Count > 0)
                        {
                            OrderMain.OrderNumber = OrderTemp.OrderNumber;
                            OrderMain.OrderID = OrderTemp.OrderID;
                            OrderMain.IsLoadFromData = true;
                            OrderMain.SubTotalVoid();
                        }
                        else { OrderMain.IsLoadFromData = false; }
                        OrderMain.UserName = session.UserName;
                        OrderMain.FloorID = FloorID;
                        OrderMain.PrintType = 1;
                        OrderMain.PDA = 99;
                        foreach (OrderItem item in listOrderItem)
                        {
                            if (OrderMain.IsLoadFromData == false)
                            {
                                if (item.ChangeStatus != 2)
                                {
                                    OrderDetailModel detailModel = new OrderDetailModel();
                                    detailModel.ProductID = Convert.ToInt32(item.ProductID);
                                    detailModel.Price = Convert.ToDouble(item.Price);
                                    detailModel.ProductName = item.ProductName;
                                    detailModel.Qty = Convert.ToInt32(item.Qty);
                                    detailModel.Total = Convert.ToDouble(item.Price);
                                    detailModel.ChangeStatus = Convert.ToInt32(item.ChangeStatus);
                                    detailModel.KeyItem = KeyItem;
                                    detailModel.Seat = Convert.ToInt32(item.Seat);
                                    detailModel.DynID = Convert.ToInt32(item.DynID);
                                    detailModel.CategoryID = Convert.ToInt32(item.CategoryID);
                                    detailModel.KeyItem = Convert.ToInt32(item.KeyIndex);
                                    var dataPrint = ProductService.GetListPrintJob(detailModel.ProductID, detailModel.CategoryID);
                                    foreach (PrinteJobDetailModel itemPrint in dataPrint)
                                    {
                                        PrinteJobDetailModel p = new PrinteJobDetailModel();
                                        p.ProductID = itemPrint.ProductID;
                                        p.PrinterID = itemPrint.PrinterID;
                                        detailModel.ListPrintJob.Add(p);
                                    }
                                    if (detailModel.ProductID == 0)
                                    {
                                        var dataPrintOpenItem = ProductService.GetListPrintJobOpenItem(detailModel.DynID);
                                        foreach (PrinteJobDetailModel itemPrint in dataPrintOpenItem)
                                        {
                                            PrinteJobDetailModel p = new PrinteJobDetailModel();
                                            p.PrinterID = Convert.ToInt32(itemPrint.PrinterID);
                                            detailModel.ListPrintJob.Add(p);
                                        }
                                    }
                                    OrderMain.addItemToList(detailModel);
                                    foreach (ModifierModel modifi in listModifier)
                                    {
                                        if (Convert.ToInt32(modifi.ProdutcID) == Convert.ToInt32(item.ProductID) && modifi.KeyModi == item.KeyIndex)
                                        {
                                            OrderDetailModifireModel modifiDetail = new OrderDetailModifireModel();
                                            modifiDetail.ProductID = Convert.ToInt32(modifi.ProdutcID);
                                            modifiDetail.ModifireName = modifi.ModifiName;
                                            modifiDetail.Qty = Convert.ToInt32(modifi.Qty);
                                            modifiDetail.ModifireID = Convert.ToInt32(modifi.ModifiID);
                                            modifiDetail.ChangeStatus = Convert.ToInt32(modifi.ChangeStatusM);
                                            modifiDetail.DynID = Convert.ToInt32(modifi.DynID);
                                            modifiDetail.Price = Convert.ToDouble(modifi.ModifiPrice);
                                            OrderMain.addModifierToList(modifiDetail, KeyItem + 1);

                                        }
                                    }
                                    KeyItem++;
                                }
                            }
                            else {
                                OrderDetailModel detailModel = new OrderDetailModel();
                                detailModel.ProductID = Convert.ToInt32(item.ProductID);
                                detailModel.Price = Convert.ToDouble(item.Price);
                                detailModel.ProductName = item.ProductName;
                                detailModel.Qty = Convert.ToInt32(item.Qty);
                                detailModel.Total = Convert.ToDouble(item.Price);
                                detailModel.ChangeStatus = Convert.ToInt32(item.ChangeStatus);
                                detailModel.KeyItem = KeyItem;
                                detailModel.Seat = Convert.ToInt32(item.Seat);
                                detailModel.DynID = Convert.ToInt32(item.DynID);
                                detailModel.CategoryID = Convert.ToInt32(item.CategoryID);
                                detailModel.KeyItem = Convert.ToInt32(item.KeyIndex);
                                var dataPrint = ProductService.GetListPrintJob(detailModel.ProductID, detailModel.CategoryID);
                                foreach (PrinteJobDetailModel itemPrint in dataPrint)
                                {
                                    PrinteJobDetailModel p = new PrinteJobDetailModel();
                                    p.ProductID = itemPrint.ProductID;
                                    p.PrinterID = itemPrint.PrinterID;
                                    detailModel.ListPrintJob.Add(p);
                                }
                                if (detailModel.ProductID == 0)
                                {
                                    var dataPrintOpenItem = ProductService.GetListPrintJobOpenItem(detailModel.DynID);
                                    foreach (PrinteJobDetailModel itemPrint in dataPrintOpenItem)
                                    {
                                        PrinteJobDetailModel p = new PrinteJobDetailModel();
                                        p.PrinterID = Convert.ToInt32(itemPrint.PrinterID);
                                        detailModel.ListPrintJob.Add(p);
                                    }
                                }
                                OrderMain.addItemToList(detailModel);
                                foreach (ModifierModel modifi in listModifier)
                                {
                                    if (Convert.ToInt32(modifi.ProdutcID) == Convert.ToInt32(item.ProductID) && modifi.KeyModi == item.KeyIndex)
                                    {
                                        OrderDetailModifireModel modifiDetail = new OrderDetailModifireModel();
                                        modifiDetail.ProductID = Convert.ToInt32(modifi.ProdutcID);
                                        modifiDetail.ModifireName = modifi.ModifiName;
                                        modifiDetail.Qty = Convert.ToInt32(modifi.ModifieQty);
                                        modifiDetail.ModifireID = Convert.ToInt32(modifi.ModifiID);
                                        modifiDetail.ChangeStatus = Convert.ToInt32(modifi.ChangeStatusM);
                                        modifiDetail.DynID = Convert.ToInt32(modifi.DynID);
                                        modifiDetail.Price = Convert.ToDouble(modifi.ModifiPrice);
                                        OrderMain.addModifierToList(modifiDetail, KeyItem + 1);

                                    }
                                }
                                KeyItem++;
                            }
                            


                        }
                        foreach (SeatModel seatItem in listSeat)
                        {
                            OrderMain.ListSeatOfOrder.Add(seatItem);
                        }
                        if (OrderMain.ListOrderDetail.Count() > 0)
                        {
                            if (checkOrderSended(OrderMain) == false)
                            {
                                return Json("NoSend", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                OrderMain.SubTotalVoid();
                                if (OrderService.InsertOrder(OrderMain) == 1)
                                {
                                    if (!OrderMain.IsLoadFromData)
                                        OrderMain.OrderID = OrderService.GetOrderID();
                                    PrinterServer printServer = new PrinterServer();
                                    printServer.PrintData(OrderMain, PrintData);
                                    return Json("1", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    else
                    {
                        return Json("BILL", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    SystemLog.LogPOS.WriteLog("OrderController::::::::::::::::::::SendOrder::::::::::::::::::" + ex.Message);
                    return null;
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return null;
           
        }
        private Boolean checkOrderSended(OrderDateModel Order)
        {
            Boolean rs = true;
            int countRemove = 0;
            for (int i = 0; i < Order.ListOrderDetail.Count; i++)
            {
                if (Order.ListOrderDetail[i].ChangeStatus == 2)
                {
                    countRemove++;
                }
            }
            if (countRemove == Order.ListOrderDetail.Count)
                rs = false;
            return rs;
        }
        public ActionResult GetTableById(string TableID)
        {
            if (Session["User"] != null)
            {
                OrderDateModel lstTable = null;
                try
                {
                    if (TableID != null)
                    {
                        OrderMain = OrderService.GetOrderByTable(TableID, 0);
                    }
                    else
                    {
                        OrderMain = OrderService.GetOrderByTable(Class.FloorID, 0);
                    }

                    int id = OrderMain.OrderID;
                    if (OrderMain.ListOrderDetail.Count > 0)
                    {
                        OrderMain.IsLoadFromData = true;
                        return Json(OrderMain, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        OrderMain.FloorID = Class.FloorID;
                        return Json(OrderMain, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    SystemLog.LogPOS.WriteLog("OrderController::::::::::::::::GetTableById::::::::::::::" + ex.Message);
                    return null;
                }
            }
            else {
                return Json("NULL", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InsertOpenItem(string lstOrderOpenItem)
        {
            try
            {
                OrderOpenItemModel OpenItem = new OrderOpenItemModel();
                int result = 0;
                List<OrderOpenItemModel> orderOpenItem = new JavaScriptSerializer().Deserialize<List<OrderOpenItemModel>>(lstOrderOpenItem);
                foreach (OrderOpenItemModel openItem in orderOpenItem)
                {
                    OpenItem.ItemNameDesc = openItem.ItemNameDesc;
                    OpenItem.ItemNameShort = openItem.ItemNameShort;
                    OpenItem.UnitPrice = Convert.ToInt32(openItem.UnitPrice);
                    if (openItem.PrinterID == "")
                        OpenItem.PrintType = "0";
                    else
                        OpenItem.PrintType = openItem.PrinterID;
                    if (openItem.PrintType == "")
                        OpenItem.PrinterID = "0";
                    else
                        OpenItem.PrinterID = openItem.PrintType;
                }
                result = OrderService.InsertOpenItem(OpenItem);
                if (result == 1)
                {
                    return Json(OrderService.GetIDLastInsertOpenItem(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                SystemLog.LogPOS.WriteLog("OrderController::::::::::::::::::::::::::::InsertOpenItem:::::::::::::::" + ex.Message);
            }
            return null;
        }
        public ActionResult CountEatIn()
        {
            var count = OrderService.CountTotalEaIn();
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        private void GetListPrinter()
        {
            PrintData.Clear();
            var listPrinter = PrintService.GetListPrinterMapping();
            foreach (PrinterModel item in listPrinter)
            {
                PrinterModel print = new PrinterModel();
                print.PrinterName = item.PrinterName;
                print.PrintName = item.PrintName;
                print.PrinterType = item.PrinterType;
                print.Header = item.Header;
                print.ID = item.ID;
                PrintData.Add(print);
            }
        }
        private void GetListPaymentPrinter()
        {
            PrintData.Clear();
            var listPrinter = PrintService.GetListPaymentprinter();

            foreach (PrinterModel item in listPrinter)
            {
                PrinterModel print = new PrinterModel();
                print.PrinterName = item.PrinterName;
                print.PrintName = item.PrintName;
                print.PrinterType = item.PrinterType;
                print.ID = item.ID;
                PrintData.Add(print);
            }
        }
        public ActionResult PrintBill()
        {
            GetListPaymentPrinter();
            OrderDateModel bill = new OrderDateModel();
            bill = OrderService.GetOrderByTable(Class.FloorID,0);
            if (bill.ListOrderDetail.Count > 0)
            {
                int result = OrderService.UpdateOrder(bill);
                if (result == 1)
                {
                    bill.PrintType = 3;
                    PrinterServer print = new PrinterServer();
                    print.PrintData(bill, PrintData);
                   
                }
            }
            return Json("Y", JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintBill1(string TableID)
        {
            GetListPaymentPrinter();
            OrderDateModel bill = new OrderDateModel();
            bill = OrderService.GetOrderByTable(TableID, 0);
            bill.UserName = Class.UserName;
            if (bill.ListOrderDetail.Count > 0)
            {
                int result = OrderService.UpdateOrder(bill);
                if (result == 1)
                {
                    bill.PrintType = 3;
                    PrinterServer print = new PrinterServer();
                    print.PrintData(bill, PrintData);

                }
            }
            return Json("Y", JsonRequestBehavior.AllowGet);
        }

    }
}
