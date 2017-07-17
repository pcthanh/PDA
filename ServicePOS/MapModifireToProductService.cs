using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
using ModelPOS.ModelEntity;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;

namespace ServicePOS
{
    public class MapModifireToProductService : IMapModifireToProduct
    {
        private POSEZ2UEntities _context;

        public MapModifireToProductService()
        {
            _context = new POSEZ2UEntities();
        }

        public MapModifireToProductService(POSEZ2UEntities context)
        {
            _context = context;
        }

        public int SaveMapModifireToProduct(List<ModifireModel> data, int productId, int userid)
        {
            try
            {
                var tabletemp = new DataTable();
                tabletemp.Columns.Add("Value");

                foreach (var item in data)
                {
                    tabletemp.Rows.Add(item.ModifireID);
                }

                var tablecategorytemp = new SqlParameter("tablemodifire", SqlDbType.Structured);
                tablecategorytemp.Value = tabletemp;
                tablecategorytemp.TypeName = "dbo.TableTemp";

                var result = _context.Database.ExecuteSqlCommand("pos_th_SaveDataMapModifireToProduct @productid,@userid,@tablemodifire",
                  new SqlParameter("productid", productId),
                  new SqlParameter("userid", userid),
                  tablecategorytemp
               );


                return 1;
            }
            catch (Exception ex)
            {
                
                return 0;
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
