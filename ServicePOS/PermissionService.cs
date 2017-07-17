using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemLog;
using ModelPOS.ModelEntity;

namespace ServicePOS
{
    public class PermissionService:IPermissionService
    { 
        #region Variables and Constructors

        private POSEZ2UEntities _context;
        public PermissionService()
        {
            _context = new POSEZ2UEntities();
        }

        public PermissionService(POSEZ2UEntities context)
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

        public int GetPermissionByDepartment(int departmentid, int menuid)
        {
            try
            {
                var data = _context.PERMISSIONs.Where(x => x.DepartmentID == departmentid && x.SubMenuID == menuid && x.Status == 1).ToList();
                return data.Count();
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("GetPermissionByDepartment :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }
           
        }

        #endregion


    }
}
