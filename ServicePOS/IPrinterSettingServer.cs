using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
   public interface IPrinterSettingServer : IDisposable
    {
        int InsertPrinterMapping(PrinteJobDetailModel PrinterJob);
        IEnumerable<PrinteJobDetailModel> GetItem(int CategoryID,int PrinterID);

        IEnumerable<PrinterModel> GetListPrinter();

        int DeletePrintJob(int Category, int ProductID, int Printer);

        int InsertPrinter(PrinterModel Printer);

        int UpdatePrinter(PrinterModel Printer);
    }
}
