using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
using ModelPOS.ModelEntity;

namespace ServicePOS
{
   public class ConfigService:IConfigService
    {


        private POSEZ2UEntities _context;
        public ConfigService()
        {
            _context = new POSEZ2UEntities();
        }
        public ConfigService(POSEZ2UEntities context)
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
        public IEnumerable<ConfigModel> GetConfig()
        {
            var list = _context.CONFIGs.Select(x => new ConfigModel() {
                Name =x.Name,
                ABN=x.ABN,
                Tel = x.Tel,
                Address = x.Address,
                Web = x.Web,
                Logan = x.Logan,
                Note =x.Note
            });
            return list;
        }

        
    }
}
