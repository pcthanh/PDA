using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelPOS.ModelEntity;
using ServicePOS.Model;

namespace ServicePOS
{
    public class ReportService : IReportService
    {
        #region Variables and Constructors

        private POSEZ2UEntities _context;
        public ReportService()
        {
            _context = new POSEZ2UEntities();
        }

        public ReportService(POSEZ2UEntities context)
        {
            _context = context;
        }

        public void SetProxyCreationEnabled(bool proxyCreationEnabled)
        {
            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
        #endregion

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

        #region New Code Report

        public IEnumerable<DailyReportModel> GetDataSummaryReport(string dateSelect,int type)
        {
            try
            {
                var data = _context.Database.SqlQuery<DailyReportModel>("pos_th_GetDataSummaryReport @dateselect,@type",
                     new SqlParameter("dateselect", dateSelect??""),
                     new SqlParameter("type", type)
                    ).ToList();

                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<QTYGroupReportModel> GetDataQTYGroupReport(string dateSelect, int type)
        {
            try
            {
                var data = _context.Database.SqlQuery<QTYGroupReportModel>("pos_th_GetQTYGroupSaleReport @dateselect,@type",
                     new SqlParameter("dateselect", dateSelect ?? ""),
                     new SqlParameter("type", type)
                    ).ToList();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<QTYItemReportModel> GetDataQTYItemReport(string dateSelect, int type)
        {
            try
            {
                var data = _context.Database.SqlQuery<QTYItemReportModel>("pos_th_GetQTYItemSaleReport @dateselect,@type",
                     new SqlParameter("dateselect", dateSelect ?? ""),
                     new SqlParameter("type", type)
                    ).ToList();

                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<ShiftReportModel> GetDataShiftReport(string dateSelect)
        {
            try
            {
                var data = _context.Database.SqlQuery<ShiftReportModel>("pos_th_GetShiftReport @dateselect",
                     new SqlParameter("dateselect", dateSelect ?? "")
                    
                    ).ToList();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<StaffSaleReportModel> GetDataStaffSaleReport(string dateSelect, int type)
        {
            try
            {
                var data = _context.Database.SqlQuery<StaffSaleReportModel>("pos_th_GetReportSaleByStaff @dateselect,@type",
                     new SqlParameter("dateselect", dateSelect ?? ""),
                     new SqlParameter("type", type)
                    ).ToList();

                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<CardSaleReportModel> GetDataCardSaleReport(string dateSelect, int type)
        {
            try
            {
                var data = _context.Database.SqlQuery<CardSaleReportModel>("pos_th_GetDataSaleByCard @dateselect,@type",
                     new SqlParameter("dateselect", dateSelect ?? ""),
                     new SqlParameter("type", type)
                    ).ToList();

                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<AccountSaleReportModel> GetDataAccountSaleReport(string dateSelect, int type)
        {
            try
            {
                var data = _context.Database.SqlQuery<AccountSaleReportModel>("pos_th_GetDataSaleByAccount @dateselect,@type",
                     new SqlParameter("dateselect", dateSelect ?? ""),
                     new SqlParameter("type", type)
                    ).ToList();

                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion New Code Report


        public string GetLinkPathSaveDataReport()
        {
            try
            {
                var datapath = _context.CONFIG_SAVE_DATA.Where(x => x.Type == 2).FirstOrDefault();
                if (datapath != null)
                    return datapath.LinkPath;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
