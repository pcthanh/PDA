using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
using ModelPOS.ModelEntity;
using SystemLog;
namespace ServicePOS
{
    public class CustomerService:ICustomerService
    {
        
        #region Variables and Constructors

        private POSEZ2UEntities _context;
        public CustomerService()
        {
            _context = new POSEZ2UEntities();
        }

        public CustomerService(POSEZ2UEntities context)
        {
            _context = context;
        }

        public void SetProxyCreationEnabled(bool proxyCreationEnabled)
        {
            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
        #endregion
        public IEnumerable<CustomerModel> GetCustomer()
        {
            var list = _context.CLIENTs.Where(x => x.Status == 1)
            .Select(x=>new CustomerModel()
             {
                 ClientID =x.ClientID,
                 Fname = x.Fname,
                 Lname=x.Lname,
                 Phone=x.Phone,
                 Email = x.Email,
                 Status = x.Status,
                 Adress1 = x.Adress1,
                 Adress2 = x.Adress2,
                 Adress3 = x.Adress3,
                 Note = x.Note,
                 Balance = x.Balance??0,
                 CreateBy = x.CreateBy,
                 CreateDate = x.CreateDate,
                 UpdateBy = x.UpdateBy,
                 UpdateDate= x.UpdateDate
             });
            return list;
        }

        public int InsertCustomer(CustomerModel item)
        {
            int result = 0;
            try
            {
                var client = new CLIENT();
                client.Lname = item.Lname;
                client.Fname = item.Fname;
                client.Phone = item.Phone;
                client.Adress1 = item.Adress1;
                client.Adress2 = item.Adress2;
                client.Adress3 = item.Adress3;
                client.Email = item.Email;
                client.Country = item.Country;
                client.Note = item.Note;
                client.Balance = item.Balance;
                client.Status = item.Status;
                client.CreateBy =item.CreateBy??0;
                client.CreateDate = DateTime.Now;
                client.UpdateBy = item.UpdateBy ?? 0;
                client.UpdateDate = DateTime.Now;
                _context.Entry(client).State = System.Data.Entity.EntityState.Added;
                _context.SaveChanges();
                result = 1;
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("CustomerService:::::::::::::::::::::InsertCustomer::::::::::::::::;;" + ex.Message);
            }
            return result;
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


        public IEnumerable<CustomerModel> GetCustomerSearch(string Name)
        {
            try
            {
                var list = _context.CLIENTs.Where(x => x.Status == 1&&x.Fname.Contains(Name))
            .Select(x => new CustomerModel()
            {
                ClientID = x.ClientID,
                Fname = x.Fname,
                Lname = x.Lname,
                Phone = x.Phone,
                Email = x.Email,
                Status = x.Status,
                Adress1 = x.Adress1,
                Adress2 = x.Adress2,
                Adress3 = x.Adress3,
                Note = x.Note,
                Balance = x.Balance ?? 0,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                UpdateBy = x.UpdateBy,
                UpdateDate = x.UpdateDate
            });
                return list;
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("CustomerService:::::::::::::::::::::::GetCustomerSearch::::::::::::::::::::" + ex.Message);
            }
            return null;
        }
    }
}
