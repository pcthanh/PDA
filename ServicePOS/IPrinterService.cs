using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
namespace ServicePOS
{
    public interface IPrinterService:IDisposable
    {
        int InsertPrinter(PrinterModel Printer);
        IEnumerable<PrinterModel> GetListPrinter();
        IEnumerable<PrinterModel> GetListPrinterNotPayment();
        int UpdatePrinter(PrinterModel Printer);

        IEnumerable<PrintJobModel> GetListPrintJobsList();

        IEnumerable<CategoryModel> GetCategoryList();
        IEnumerable<ProductionModel> GetProductListByCategory(int categoryid);

        IEnumerable<PrintJobDetailModel> GetPrintJobDetailList(int PrinteJobID);


        int SaveDataPrinterJob(PrintJobModel data);

        int RemoveDataPrinterJob(PrintJobModel data);
        IEnumerable<PrinterModel> GetListPrinterMapping();
        IEnumerable<PrinterModel> GetListPaymentprinter();

        IEnumerable<PrinterModel> GetListPrinterJoinTable();

        IEnumerable<PrinterModel> GetListPrinterTransferTable();

        IEnumerable<PrinterModel> GetListPrinterReport();
        IEnumerable<PrinterModel> GetListPrinterNote();

    }
}
