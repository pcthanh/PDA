using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface IProductService : IDisposable
    {
        IEnumerable<ProductionModel> GetProductsList(int CurrentPage);
        int GetTotalProducts();
        int Created(ProductionModel product);
        int Delete(ProductionModel product);
        IEnumerable<ProductionModel> GetProdutcByCategory(int id,int page);
        ProductionModel GetPrinterType(int ID);
        ProductionModel GetPrinterTypeByCate(int ID);
        IEnumerable<ProductionModel> searchProduct(string textSearch, int type);
        IEnumerable<ProductionModel> GetProdutcByCategoryPrint(int id);

        IEnumerable<PrinteJobDetailModel> GetListPrintJob(int ProductID, int catagory);
        IEnumerable<PrinteJobDetailModel> GetListPrintJobOpenItem(int dynID);
    }
}
