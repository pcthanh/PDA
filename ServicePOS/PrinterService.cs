using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelPOS.ModelEntity;
using ServicePOS.Model;
using ModelPOS;
using SystemLog;

namespace ServicePOS
{
   public class PrinterService:IPrinterService
    {
       public string className="PrinterService:::::::::::::::::::::::";
        private POSEZ2UEntities _context;
     
        public PrinterService()
        {
            _context = new POSEZ2UEntities();
          
        }

        public PrinterService(POSEZ2UEntities context)
        {
            _context = context;
        }
        public void SetProxyCreationEnabled(bool proxyCreationEnabled)
        {
            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }


        #region Printer
        public int InsertPrinter(Model.PrinterModel Printer)
        {
            int result = 0;
            try
            {
                if (Printer != null)
                {
                    var item = new PRINTER();
                    item.PrinterName = Printer.PrinterName;
                    item.PrintName = Printer.PrintName;
                    item.PrinterType = Printer.PrinterType;
                    item.Status = Printer.Status;
                    item.CreateBy = Printer.CreateBy ?? 0;
                    item.CreateDate = DateTime.Now;
                    item.UpdateBy = Printer.UpdateBy ?? 0;
                    item.UpdateDate = DateTime.Now;
                    _context.Entry(item).State = System.Data.Entity.EntityState.Added;
                    _context.SaveChanges();
                    result = 1;
                }
            }
            catch (Exception ex) 
            {
                LogPOS.WriteLog(className+"InsertPrinter:::::::::::::::::::;;"+ ex.Message);
            }
            return result;
        }

        public IEnumerable<Model.PrinterModel> GetListPrinter()
        {
            
            try
            {
               var data= _context.PRINTERs.Where(x=>x.Status==1).Select
                   (x=>new PrinterModel()
                   {
                       ID = x.ID,
                       PrinterName = x.PrinterName,
                       PrintName =x.PrintName,
                       PrinterType=x.PrinterType??0,
                       Status=x.Status,
                       NumberOfTicket = x.NumberOfTicket??0,
                       Header = x.Header,
                       CreateBy=x.CreateBy,
                       CreateDate=x.CreateDate,
                       UpdateBy=x.UpdateBy,
                       UpdateDate=x.UpdateDate
                   }
                   );
                 return data;
            }
             
            catch(Exception ex)
            {

            }
            return null;
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
                LogPOS.WriteLog(className+"InsertPrinter:::::::::::::::::::;;"+ ex.Message);
            }
            return result;
        }

        #endregion Printer


        #region Printer Job

        public IEnumerable<PrintJobModel> GetListPrintJobsList()
       {
           try
           {
               var data = _context.PRINT_JOB.Where(x => x.Status == 1).Select
                   (x => new PrintJobModel()
                   {
                       ID = x.ID,
                       PrintJobName = x.PrintJobName??"",
                       PrintContent = x.PrintContent??"",
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
               return null;
           }
       }

       public IEnumerable<CategoryModel> GetCategoryList()
       {
           var data = _context.CATEGORies.Where(x => x.Status == 1).Select(x => new CategoryModel
           {
               CategoryID = x.CategoryID,
               CategoryName = x.CategoryName
           });

           return data;
       }


       public IEnumerable<ProductionModel> GetProductListByCategory(int categoryid)
       {
           var data = _context.MAP_PRODUCT_TO_CATEGORY.
               Join(_context.PRODUCTs, map => map.ProductID, product => product.ProductID, (map, product) => new { map, product })
               .Where(x => x.map.Status == 1 && x.product.Status==1 && x.map.CategoryID==categoryid).Select(x => new ProductionModel
           {
               ProductID = x.product.ProductID,
               ProductNameDesc = x.product.ProductNameDesc
           });

           return data;
       }


       public IEnumerable<PrintJobDetailModel> GetPrintJobDetailList(int PrinteJobID)
       {
           //var data =
           //    _context.PRINTE_JOB_DETAIL.Where(x => x.PrinteJobID == PrinteJobID)
           //        .Select(x => new PrintJobDetailModel
           //        {
           //            ID = x.ID,
           //            PrinteJobID = x.PrinteJobID,
           //            CategoryID = x.CategoryID,
           //            ProductID = x.ProductID,
           //            PrinterID = x.PrinterID,
           //            TemplatesID = x.TemplatesID,
           //            Status = x.Status,
           //            Notes = x.Notes??"",
           //            CreateBy = x.CreateBy,
           //            CreateDate = x.CreateDate,
           //            UpdateBy = x.UpdateBy,
           //            UpdateDate = x.UpdateDate
           //        });

           //return data;
           return null;
       }

       public int SaveDataPrinterJob(PrintJobModel data)
       {
           try
           {
               #region Update 
               if (data.ID > 0)
               {
                   var printejob = _context.PRINT_JOB.Find(data.ID);
                   if (printejob != null)
                   {
                       printejob.PrintJobName = data.PrintJobName;
                       printejob.PrintContent = data.PrintContent;

                       printejob.UpdateBy = data.UpdateBy;
                       printejob.UpdateDate = DateTime.Now;

                       _context.Entry(printejob).State = System.Data.Entity.EntityState.Modified;
                       _context.SaveChanges();

                       foreach (var item in data.dataDetail)
                       {
                           if (item.ID > 0)
                           {
                               var tempitem = _context.PRINTE_JOB_DETAIL.Find(item.ID);
                               if (tempitem!=null)
                               {
                                   tempitem.CategoryID = item.CategoryID;
                                   tempitem.ProductID = item.ProductID;
                                   tempitem.PrinterID = item.PrinterID;
                                   tempitem.TemplatesID = item.TemplatesID;

                                   tempitem.UpdateBy = item.UpdateBy;
                                   tempitem.UpdateDate = DateTime.Now;

                                   _context.Entry(tempitem).State = System.Data.Entity.EntityState.Modified;
                                   _context.SaveChanges();

                               }
                           }
                           else
                           {
                               var tempitem = new PRINTE_JOB_DETAIL();

                               //tempitem.PrinteJobID = printejob.ID;

                               tempitem.CategoryID = item.CategoryID;
                               tempitem.ProductID = item.ProductID;
                               tempitem.PrinterID = item.PrinterID;
                               tempitem.TemplatesID = item.TemplatesID;

                               tempitem.Status = 1;

                               tempitem.Notes = item.Notes??"";

                               tempitem.CreateBy = item.CreateBy;
                               tempitem.CreateDate = DateTime.Now;

                               _context.Entry(tempitem).State = System.Data.Entity.EntityState.Added;
                               _context.SaveChanges();
                           }
                       }

                       return 1;
                   }
                   return 0;
               }

               #endregion Update

               #region Insert
               else
               {
                   var printejob = new PRINT_JOB();

                   printejob.PrintJobName = data.PrintJobName;
                   printejob.PrintContent = data.PrintContent;

                   printejob.Status = 1;

                   printejob.CreateBy = data.CreateBy;
                   printejob.CreateDate = DateTime.Now;

                   _context.Entry(printejob).State = System.Data.Entity.EntityState.Added;
                   _context.SaveChanges();

                   foreach (var item in data.dataDetail)
                   {

                       var tempitem = new PRINTE_JOB_DETAIL();

                       //tempitem.PrinteJobID = printejob.ID;

                       tempitem.CategoryID = item.CategoryID;
                       tempitem.ProductID = item.ProductID;
                       tempitem.PrinterID = item.PrinterID;
                       tempitem.TemplatesID = item.TemplatesID;

                       tempitem.Status = 1;

                       tempitem.Notes = item.Notes ?? "";

                       tempitem.CreateBy = item.CreateBy;
                       tempitem.CreateDate = DateTime.Now;

                       _context.Entry(tempitem).State = System.Data.Entity.EntityState.Added;
                       _context.SaveChanges();
                   }

                   return 1;
               }
               #endregion Insert

           }
           catch (Exception ex)
           {
               LogPOS.WriteLog("Service Printer Job:::::::::::::::::::SaveDataPrinterJob::::::::::::::::" + ex.Message);
               return 0;
           }
          
       }


       public int RemoveDataPrinterJob(PrintJobModel data)
       {
           try
           {
               //if (data!=null)
               //{
               //    var printjob = _context.PRINT_JOB.Find(data.ID);
               //    if (printjob != null)
               //    {
               //        printjob.Status = 0;
               //        printjob.UpdateBy = data.UpdateBy;
               //        printjob.UpdateDate = DateTime.Now;

               //        _context.Entry(printjob).State = System.Data.Entity.EntityState.Modified;
               //        _context.SaveChanges();

               //        var detail =_context.PRINTE_JOB_DETAIL.Where(x => x.PrinteJobID == printjob.ID && x.Status == 1).ToList();
               //        foreach (var item in detail)
               //        {
               //            item.Status = 0;
               //            item.UpdateBy = data.UpdateBy;
               //            item.UpdateDate = DateTime.Now;

               //            _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
               //            _context.SaveChanges();
               //        }
               //        return 1;
               //    }
               //}
               //return 0;
           }
           catch (Exception ex)
           {
               LogPOS.WriteLog("Service Printer Job:::::::::::::::::::RemoveDataPrinterJob::::::::::::::::" + ex.Message);
               return 0;
           }
           return 0;
       }

        #endregion Printer Job


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


        public IEnumerable<PrinterModel> GetListPrinterMapping()
        {
            var data = _context.PRINTERs.Where(x => x.Status == 1 && x.PrinterType==1)
                .Select(x => new PrinterModel
                {
                    PrinterName = x.PrinterName,
                    PrintName = x.PrintName,
                    PrinterType = x.PrinterType??0,
                    ID = x.ID,
                    Header = x.Header
                }
                );
            return data;
        }


        public IEnumerable<PrinterModel> GetListPaymentprinter()
        {
            var data = _context.PRINTERs.Where(x => x.Status == 1 &&x.PrinterType==4)
               .Select(x => new PrinterModel
               {
                   PrinterName = x.PrinterName,
                   PrintName = x.PrintName,
                   PrinterType = x.PrinterType??0,
                   ID = x.ID
               }
               );
            return data;
        }


        public IEnumerable<PrinterModel> GetListPrinterNotPayment()
        {


            try
            {
                var data = _context.PRINTERs.Where(x => x.Status == 1 && x.PrinterType==1).Select
                    (x => new PrinterModel()
                    {
                        ID = x.ID,
                        PrinterName = x.PrinterName,
                        PrintName = x.PrintName,
                        PrinterType = x.PrinterType??0,
                        Status = x.Status,
                        CreateBy = x.CreateBy,
                        CreateDate = x.CreateDate,
                        UpdateBy = x.UpdateBy,
                        UpdateDate = x.UpdateDate,
                        Text = x.PrintName,
                        Value = x.ID
						
                    }
                    );
                return data;
            }

            catch (Exception ex)
            {

            }
            return null;
        }


        public IEnumerable<PrinterModel> GetListPrinterJoinTable()
        {
            try
            {
                var data = _context.PRINTERs.Where(x => x.Status == 1 && x.PrinterType==2).Select
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
                        UpdateDate = x.UpdateDate,
                        Header = x.Header

                    }
                    );
                return data;
            }

            catch (Exception ex)
            {

            }
            return null;
        }




        public IEnumerable<PrinterModel> GetListPrinterTransferTable()
        {
            try
            {
                var data = _context.PRINTERs.Where(x => x.Status == 1 && x.PrinterType == 3).Select
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
                        UpdateDate = x.UpdateDate,
                        Header = x.Header

                    }
                    );
                return data;
            }

            catch (Exception ex)
            {

            }
            return null;
        }


        public IEnumerable<PrinterModel> GetListPrinterReport()
        {
            try
            {
                var data = _context.PRINTERs.Where(x => x.Status == 1 && x.PrinterType == 5).Select
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
                        UpdateDate = x.UpdateDate,
                        Header = x.Header

                    }
                    );
                return data;
            }

            catch (Exception ex)
            {
                return null;
            }

        }


        public IEnumerable<PrinterModel> GetListPrinterNote()
        {
            try
            {
                var data = _context.PRINTERs.Where(x => x.Status == 1 && x.PrinterType == 1 ||x.PrinterType==0).Select
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
    }
}
