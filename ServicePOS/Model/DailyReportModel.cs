using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class DailyReportModel
    {
        public int NetSale { get; set; }

        public int GST { get; set; }
        public int Discount { get; set; }

        public int Change { get; set; }
        public int Refund { get; set; }

        public double CashTotal { get; set; }
        public double CardTotal { get; set; }

        public double ChequeTotal { get; set; }
        public double AccountTotal { get; set; }

        public double GrifCardTotal { get; set; }
        public double SaleTotal { get; set; }
    }

    public class QTYGroupReportModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public double TotalQty { get; set; }
    }

    public class QTYItemReportModel
    {
        public int ProductID { get; set; }
        public string ProductNameDesc { get; set; }
        public double TotalQty { get; set; }
    }

    public class ShiftReportModel
    {
        public int ShiftHistoryID { get; set; }
        public string ShiftName { get; set; }
        public string UserName { get; set; }
        public Nullable<DateTime> StartShift { get; set; }

        public Nullable<DateTime> EndShift { get; set; }

        public double CashStart { get; set; }

        public double CashEnd { get; set; }
        public double SafeDrop { get; set; }

        public double TotalCash { get; set; }

        public int TotalSale { get; set; }

    }

    public class StaffSaleReportModel
    {
        public int StaffID { get; set; }
        public string UserName { get; set; }
        public int Total { get; set; }
    }

    public class CardSaleReportModel
    {
        public int CardID { get; set; }
        public string CardName { get; set; }
        public double Total { get; set; }
    }

    public class AccountSaleReportModel
    {
        public int ClientID { get; set; }
        public string FullName { get; set; }
        public int Total { get; set; }
    }
}
