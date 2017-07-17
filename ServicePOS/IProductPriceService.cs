using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface IProductPriceService : IDisposable
    {
        IEnumerable<ProductPriceModel> GetListProductPrice();

        IEnumerable<ProductPriceModel> GetDetailProductPrice(int productID);

        int EditProductPrice(ProductPriceModel productPriceData);
    }
}
