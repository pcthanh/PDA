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
    public class DatabaseSettingService : IDatabaseSettingService
    {
        #region Variables and Constructors

        private POSEZ2UEntities _context;
        public DatabaseSettingService()
        {
            _context = new POSEZ2UEntities();
        }

        public DatabaseSettingService(POSEZ2UEntities context)
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


        #region Function

        public IEnumerable<BackupDataModel> GetDataBackupFile()
        {

            try
            {
                var data = _context.Database.SqlQuery<BackupDataModel>("pos_th_GetDataBackupDatabase").ToList();

                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }


        public int BackupDatabaseSetting(int userid)
        {

            try
            {
                var datapath = _context.CONFIG_SAVE_DATA.Where(x => x.Type == 1).FirstOrDefault();
                string filepath = "F:\\POSEZ2U_" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + ".Bak";
                
                if (datapath != null)
                {
                    filepath = datapath.LinkPath + "POSEZ2U_" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + ".Bak";
                }

                string dbname = _context.Database.Connection.Database;
                string sqlCommand = @"BACKUP DATABASE POSEZ2U TO DISK = '" + filepath + "'";
                _context.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname, filepath));


                var datatable = new DATABASE_BACKUP();
                datatable.FileName = filepath;
                datatable.Notes = filepath;
                datatable.CreateBy = userid;
                datatable.CreateDate = DateTime.Now;

                _context.Entry(datatable).State = System.Data.Entity.EntityState.Added;
                _context.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }



        #endregion
    }
}
