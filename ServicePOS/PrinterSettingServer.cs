using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelPOS.ModelEntity;
using ModelPOS;
using ServicePOS.Model;
using SystemLog;

namespace ServicePOS
{
    public class PrinterSettingServer : IPrinterSettingServer
    {

        private POSEZ2UEntities _context;

        public PrinterSettingServer()
        {
            _context = new POSEZ2UEntities();
        }

        public PrinterSettingServer(POSEZ2UEntities context)
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
        

        public int InsertPrinterMapping(Model.PrinteJobDetailModel PrinterJob)
        {
            int result = 0;
            try
            {
                PRINTE_JOB_DETAIL item = new PRINTE_JOB_DETAIL();
                item.CategoryID = PrinterJob.CategoryID;
                item.ProductID = PrinterJob.ProductID;
                item.PrinterID = PrinterJob.PrinterID;
                item.Status = 1;
                _context.Entry(item).State = System.Data.Entity.EntityState.Added;
                _context.SaveChanges();
                result = 1;
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }



        public IEnumerable<PrinteJobDetailModel> GetItem(int CategoryID, int PrinterID)
        {
            var data = _context.PRINTE_JOB_DETAIL.Where(x => x.Status == 1 && x.CategoryID==CategoryID && x.PrinterID==PrinterID)
                .Select(x => new PrinteJobDetailModel{ 
                    CategoryID = x.CategoryID,
                    PrinterID =x.PrinterID,
                    ProductID =x.ProductID
                });
            return data;
                
        }


        public int DeletePrintJob(int Category, int ProductID, int Printer)
        {
            int result = 0;
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    _context.Database.ExecuteSqlCommand("delete from PRINTE_JOB_DETAIL where CategoryID='" + Category + "' and ProductID='" + ProductID + "' and PrinterID='" + Printer + "'");
                    trans.Commit();
                    result = 1;
                }
            }
            
            catch(Exception ex)
            {
                
            }
            return result;
        }


        public IEnumerable<PrinterModel> GetListPrinter()
        {
            try
            {
                var data = _context.PRINTERs.Where(x => x.Status == 1).Select
                    (x => new PrinterModel()
                    {
                        ID = x.ID,
                        PrinterName = x.PrinterName,
                        PrintName = x.PrintName,
                        PrinterType = x.PrinterType ?? 0,
                        Status = x.Status,
                        CreateBy = x.CreateBy,
                        CreateDate = x.CreateDate,
                        UpdateBy = x.UpdateBy,
                        UpdateDate = x.UpdateDate
                    }
                    );
                return data;
            }

            catch (Exception ex)
            {

            }
            return null;
        }


        public int InsertPrinter(PrinterModel Printer)
        {
            int result = 0;
            using (var trans = _context.Database.BeginTransaction())
            {
                PRINTER printer = new PRINTER();
                printer.PrinterName = Printer.PrinterName;
                printer.PrintName = Printer.PrintName;
                printer.PrinterType = Printer.PrinterType;
                printer.Status = 1;
                printer.Header = Printer.Header;
                printer.NumberOfTicket = Printer.NumberOfTicket;
                printer.CreateBy = Printer.CreateBy ?? 0;
                printer.CreateDate = DateTime.Now;
                _context.Entry(printer).State = System.Data.Entity.EntityState.Added;
                _context.SaveChanges();
                trans.Commit();
                result = 1;
            }
            return result;
        }


        public int UpdatePrinter(PrinterModel Printer)
        {
            int result = 0;
            try
            {
                if (Printer != null)
                {
                    var item = _context.PRINTERs.Find(Printer.ID);
                    if (item != null)
                    {
                        item.ID = Printer.ID;
                        item.PrinterName = Printer.PrinterName;
                        item.PrintName = Printer.PrintName;
                        item.PrinterType = Printer.PrinterType;
                        item.Status = Printer.Status;
                        item.Header = Printer.Header;
                        item.NumberOfTicket = Printer.NumberOfTicket;
                        //item.CreateBy = Printer.CreateBy ?? 0;
                        //item.CreateDate = DateTime.Now;
                        item.UpdateBy = Printer.UpdateBy ?? 0;
                        item.UpdateDate = DateTime.Now;
                        _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        result = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("PrinterSettingServer::::::::::::::InsertPrinter:::::::::::::::::::" + ex.Message);
            }
            return result;
        }
    }
}
    

