using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface IReportService:IDisposable
    {
        #region New Code Report

        IEnumerable<DailyReportModel> GetDataSummaryReport(string dateSelect, int type);

        IEnumerable<QTYGroupReportModel> GetDataQTYGroupReport(string dateSelect, int type);
        IEnumerable<QTYItemReportModel> GetDataQTYItemReport(string dateSelect, int type);

        IEnumerable<ShiftReportModel> GetDataShiftReport(string dateSelect);

        IEnumerable<StaffSaleReportModel> GetDataStaffSaleReport(string dateSelect, int type);

        IEnumerable<CardSaleReportModel> GetDataCardSaleReport(string dateSelect, int type);

        IEnumerable<AccountSaleReportModel> GetDataAccountSaleReport(string dateSelect, int type);

        #endregion New Code Report

        string GetLinkPathSaveDataReport();
    }
}
