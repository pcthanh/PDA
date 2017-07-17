using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelPOS.ModelEntity;
using ServicePOS.Model;
using SystemLog;

namespace ServicePOS
{
   public class AccountPaymentService:IAccountPaymentService
    {


        private POSEZ2UEntities _context;
        public AccountPaymentService()
        {
            _context = new POSEZ2UEntities();
        }
        public AccountPaymentService(POSEZ2UEntities context)
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

        public int InsertDebitAccount(AccountPaymentModel acc)
        {
            int result = 0;
            try
            {
                using (var tranx = _context.Database.BeginTransaction())
                {
                    ACC_PAYMENT Acc = new ACC_PAYMENT();
                    Acc.PayMentID = acc.PayMentID;
                    Acc.InvoiceID = 0;
                    Acc.InvoiceNumber = "0";
                    Acc.SubTotal = acc.SubTotal;
                    Acc.Card = acc.Card;
                    Acc.Cash = acc.Cash;
                    Acc.IsCredit = 0;
                    Acc.IsDebit = 1;
                    Acc.CreateBy = 0;
                    Acc.CreateDate = DateTime.Now;
                    Acc.UpdateBy = 0;
                    Acc.UpdateDate = DateTime.Now;
                    Acc.CusNo = acc.CusNo;
                    _context.Entry(Acc).State = System.Data.Entity.EntityState.Added;
                    _context.SaveChanges();
                    string sql = "update client set balance=balance+'" + Convert.ToInt32(acc.SubTotal) + "' where ClientID='" + acc.CusNo + "'";
                    _context.Database.ExecuteSqlCommand(sql);
                    tranx.Commit();
                    result = 1;
                }
                

            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("AccountPaymentService:::::::::::::::::::::::::::InsertDebitAccount::::::::::::::" + ex.Message);
            }
            return result;
            
        }



        public IEnumerable<AccountPaymentModel> GetAccPayment(int CusNo, DateTime TimeFrom, DateTime TimeTo)
        {
            var list  = _context.ACC_PAYMENT.Where(x=>x.CusNo==CusNo && x.CreateDate>=TimeFrom && x.CreateDate<=TimeTo)
                .Select(x=>new AccountPaymentModel()
            {
                InvoiceID =x.InvoiceID,
                InvoiceNumber = x.InvoiceNumber,
                SubTotal = x.SubTotal,
                Cash =x.Cash,
                Card=x.Card,
                IsCredit = x.IsCredit,
                IsDebit = x.IsDebit,
                CreateDate = x.CreateDate
            });
            return list;
        }
    }
}
