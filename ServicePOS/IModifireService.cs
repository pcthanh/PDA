using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface IModifireService:IDisposable
    {
        IEnumerable<ModifireModel> GetModifireList(int CurrentPage);

        int GetTotalModifire();

        int Created(ModifireModel data);

        int Delete(ModifireModel data);

        IEnumerable<ModifireModel> GetModifireAllList(int productID);
        IEnumerable<ModifireModel> GetSearchModifireAllList(int productID, string txtSearch);


        IEnumerable<ModifireModel> GetListModifireToProduct(int productID);
        IEnumerable<ModifireModel> GetListModifireToProduct(int productID, int CurrentPage);

        IEnumerable<ModifireModel> GetModifireByProduct(int productID,int Page);

        IEnumerable<ModifireModel> searchProduct(string textSearch, int type);

        int EditModifirePrice(ModifirePriceModel modifirePriceData);
    }
}
