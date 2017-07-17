using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface IShiftService:IDisposable
    {
        IEnumerable<StaffModel> GetAllStaffActive();
        int InsertDataShiftHistory(ShiftHistoryModel model);
        int UpdateDataShiftHistory(ShiftHistoryModel model);

        IEnumerable<ShiftHistoryModel> GetListShiftHistoryByUserid(int userid, int type);

        int CountShiftWorking();
    }
}
