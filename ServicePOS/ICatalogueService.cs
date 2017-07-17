
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface ICatalogueService:IDisposable
    {

        IEnumerable<CatalogueModel> GetCatalogueList();

        int SavaDataCatalogue(CatalogueModel cata);

        int RemoveCatalogue(int catalogueid, int userid);

        IEnumerable<CategoryModel> GetCategoryByCatalogueID(int CatalogueID);
        IEnumerable<CategoryModel> GetCategoryByCatalogueID(int CatalogueID, int currentPage);
        int SaveMapCategoryToCatalogue(List<CategoryModel> data, int catalogueid,int userid);
        IEnumerable<CategoryModel> GetAllListCategoryByCatalogue(int CatalogueID);
        IEnumerable<CategoryModel> GetSearchAllListCategoryByCatalogue(int CatalogueID, string txtSearch);


        IEnumerable<CategoryModel> GetListCategory();
        IEnumerable<CategoryModel> GetListCategory(int CurrentPage);
        int GetTotalCategory();
        int SaveDataCategory(CategoryModel cate);
        int RemoveCategory(int categoryid, int userid);

        IEnumerable<ProductionModel> GetProductByCategoryID(int CategoryID);
        IEnumerable<ProductionModel> GetProductByCategoryID(int CategoryID, int currentPage);

        IEnumerable<ProductionModel> GetAllListProductByCategory(int CategoryID);
        IEnumerable<ProductionModel> GetSearchAllListProductByCategory(int CategoryID, string textSearch);

        int SaveDataMapProductToCategory(List<ProductionModel> data, int categoryid, int userid);

        IEnumerable<CategoryModel> searchProduct(string textSearch, int type);
    }
}
