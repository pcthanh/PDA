using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelPOS.ModelEntity;
using ServicePOS.Model;
using System.Data.Entity;
using System.Data.SqlClient;

namespace ServicePOS
{
    public class PriceListService : IPriceListService
    {
        #region Variables and Constructors
        private POSEZ2UEntities _context;

        public PriceListService()
        {
            _context = new POSEZ2UEntities();
        }

        public PriceListService(POSEZ2UEntities context)
        {
            _context = context;
        }

        public void SetProxyCreationEnabled(bool proxyCreationEnabled)
        {
            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
        #endregion

        public IEnumerable<PriceListModel> GetDataProductAndModifire(int CurrentPage)
        {
            var data = _context.Database.SqlQuery<PriceListModel>("pos_th_GetDataProductAndModifire")
                .OrderBy(p => p.NameDesc).Skip(10 * (CurrentPage - 1))
                .Take(10).ToList();
            return data;
        }

        public int GetTotalProductAndModifire()
        {
            var data = _context.Database.SqlQuery<PriceListModel>("pos_th_GetDataProductAndModifire")
                .Count();
            return data;
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
