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
    public class ProductPriceService : IProductPriceService
    {
        private POSEZ2UEntities _context;

        public ProductPriceService()
        {
            _context = new POSEZ2UEntities();
        }

        public ProductPriceService(POSEZ2UEntities context)
        {
            _context = context;
        }


        public IEnumerable<ProductPriceModel> GetListProductPrice()
        {
            var data = _context.Database.SqlQuery<ProductPriceModel>("pos_th_GetListProductPrice");
            return data;
        }

        public IEnumerable<ProductPriceModel> GetDetailProductPrice(int productPriceID)
        {
            var data = _context.Database.SqlQuery<ProductPriceModel>("pos_th_GetDetailProductPrice @productpriceID", new SqlParameter("productpriceID", productPriceID));
            return data;
        }

        public int EditProductPrice(ProductPriceModel productPriceData)
        {
            var data = _context.PRODUCT_PRICE.Find(productPriceData.ProductPriceID);
            if (data != null)
            {
                data.WasPrice = data.CurrentPrice;
                data.CurrentPrice = productPriceData.CurrentPrice;
                data.UpdateBy = productPriceData.UpdateBy ?? 0;
                data.UpdateDate = DateTime.Now;
                _context.Entry(data).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            else
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
