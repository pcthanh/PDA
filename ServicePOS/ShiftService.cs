using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemLog;
using ModelPOS.ModelEntity;
using ServicePOS.Model;

namespace ServicePOS
{
    public class ShiftService : IShiftService
    {
        #region Variables and Constructors

        private POSEZ2UEntities _context;
        public ShiftService()
        {
            _context = new POSEZ2UEntities();
        }

        public ShiftService(POSEZ2UEntities context)
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

        #region Function All

        public IEnumerable<StaffModel> GetAllStaffActive()
        {
            var data = _context.STAFFs.Where(x => x.Status == 1).Select(x => new StaffModel
            {
                StaffID = x.StaffID,
                UserName = x.UserName,
                Fname = x.Fname,
                Lname = x.Lname
            });
            return data;
        }

        public int InsertDataShiftHistory(ShiftHistoryModel model)
        {
            try
            {
                var data = new SHIFT_HISTORY();

                data.ShiftName = model.ShiftName ?? "";
                data.StaffID = model.StaffID;
                data.CashStart = model.CashStart;

                data.Status = 1;
                data.StartShift = DateTime.Now;

                data.CreateBy = model.CreateBy;
                data.CreateDate = DateTime.Now;

                _context.Entry(data).State = EntityState.Added;
                _context.SaveChanges();


                return data.ShiftHistoryID;
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("SaveDataStaff :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }
        }

        public int UpdateDataShiftHistory(ShiftHistoryModel model)
        {
            try
            {
                if (model.ShiftHistoryID > 0)
                {
                    var data = _context.SHIFT_HISTORY.Find(model.ShiftHistoryID);
                    if (data != null)
                    {

                        data.EndShift = DateTime.Now;

                        data.CashEnd = model.CashEnd;

                        data.SafeDrop = model.SafeDrop;

                        data.UpdateBy = model.UpdateBy;
                        data.UpdateDate = DateTime.Now;

                        data.Status = 2;

                        _context.Entry(data).State = EntityState.Modified;
                        _context.SaveChanges();


                        return data.ShiftHistoryID; 
                    }
                    return 0;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("SaveDataStaff :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }
        }


        public IEnumerable<ShiftHistoryModel> GetListShiftHistoryByUserid(int userid, int type)
        {

            try
            {
                var data = _context.Database.SqlQuery<ShiftHistoryModel>("pos_th_GetDataListShiftHistoryByUserID @userid,@type",
                    new SqlParameter("userid", userid),
                    new SqlParameter("type", type)
                ).ToList();

                return data;
            }
            catch (Exception ex)
            {
                SystemLog.LogPOS.WriteLog("ShiftService::::::::::::::GetListShiftHistoryByUserid::::::::::::::::" + ex.Message);
                return null;
            }

        }
        #endregion


        public int CountShiftWorking()
        {
            return _context.SHIFT_HISTORY.Where(x => x.Status == 1).Count();
        }
    }
}
