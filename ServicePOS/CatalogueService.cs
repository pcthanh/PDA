
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ModelPOS.ModelEntity;
using ServicePOS.Model;

namespace ServicePOS
{
    public class CatalogueService : ICatalogueService
    {

        #region Variables and Constructors

        private POSEZ2UEntities _context;
        public CatalogueService()
        {
            _context = new POSEZ2UEntities();
        }

        public CatalogueService(POSEZ2UEntities context)
        {
            _context = context;
        }

        public void SetProxyCreationEnabled(bool proxyCreationEnabled)
        {
            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
        #endregion


        #region Function


        #region Catalogue
        public IEnumerable<CatalogueModel> GetCatalogueList()
        {
            var data = _context.CATALOGUEs.Where(x => x.Status == 1).Select(x => new CatalogueModel
            {
                CatalogueID = x.CatalogueID,
                CatalogueName = x.CatalogueName,
                Status = x.Status,
                Color = x.Color ?? ""
            });
            return data;
        }


        public int SavaDataCatalogue(CatalogueModel cata)
        {
            try
            {

                if (cata.CatalogueID == 0)
                {
                    var checkdata = _context.CATALOGUEs.Where(x => x.CatalogueName == cata.CatalogueName).ToList();
                    if (checkdata.Count() > 0)
                    {
                        return -1;
                    }

                    var data = new CATALOGUE();
                    data.CatalogueName = cata.CatalogueName;
                    data.Color = cata.Color ?? "";
                    data.Status = 1;
                    data.CreateBy = cata.CreateBy ?? 0;
                    data.CreateDate = DateTime.Now;
                    data.UpdateBy = cata.UpdateBy ?? 0;
                    data.UpdateDate = DateTime.Now;
                    data.Note = cata.Note ?? "";

                    _context.Entry(data).State = EntityState.Added;
                    _context.SaveChanges();

                    return 1;

                }
                else
                {
                    var checkdata = _context.CATALOGUEs.Where(x => x.CatalogueName == cata.CatalogueName && x.CatalogueID != cata.CatalogueID).ToList();
                    if (checkdata.Count() > 0)
                    {
                        return -1;
                    }
                    var data = _context.CATALOGUEs.Find(cata.CatalogueID);
                    if (data != null)
                    {
                        data.CatalogueName = cata.CatalogueName;
                        data.Color = cata.Color ?? "";
                        data.Status = 1;
                        data.UpdateBy = cata.UpdateBy ?? 0;
                        data.UpdateDate = DateTime.Now;
                        data.Note = cata.Note ?? "";

                        _context.Entry(data).State = EntityState.Modified;
                        _context.SaveChanges();

                        return 1;
                    }
                    return 0;

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int RemoveCatalogue(int catalogueid, int userid)
        {
            var data = _context.CATALOGUEs.Find(catalogueid);
            {
                if (data != null)
                {
                    data.Status = 0;
                    data.UpdateBy = userid;
                    data.UpdateDate = DateTime.Now;
                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return 1;
                }
            }
            return 0;
        }


        public IEnumerable<CategoryModel> GetCategoryByCatalogueID(int CatalogueID)
        {

            var data = _context.MAP_CATEGORY_TO_CATALOGUE.Join(_context.CATEGORies, cata => cata.CategoryID,
                cate => cate.CategoryID, (cata, cate) => new { cata, cate })
                .Where(x => x.cata.CatalogueID == CatalogueID && x.cata.Status == 1)
                .Select(x => new CategoryModel()
                {
                    CatalogueID = x.cata.CatalogueID,
                    CategoryID = x.cate.CategoryID,
                    CategoryName = x.cate.CategoryName,
                    Status = x.cate.Status
                });

            return data;
        }

        public IEnumerable<CategoryModel> GetCategoryByCatalogueID(int CatalogueID, int currentPage)
        {

            var data = _context.MAP_CATEGORY_TO_CATALOGUE.Join(_context.CATEGORies, cata => cata.CategoryID,
                cate => cate.CategoryID, (cata, cate) => new { cata, cate })
                .Where(x => x.cata.CatalogueID == CatalogueID && x.cata.Status == 1)
                .Select(x => new CategoryModel()
                {
                    CatalogueID = x.cata.CatalogueID,
                    CategoryID = x.cate.CategoryID,
                    CategoryName = x.cate.CategoryName,
                    Status = x.cate.Status
                }).OrderBy(p => p.CategoryName).Skip(10 * (currentPage - 1))
                .Take(10).ToList();
            return data;
        }

        public IEnumerable<CategoryModel> GetAllListCategoryByCatalogue(int CatalogueID)
        {
            var data = _context.Database.SqlQuery<CategoryModel>("pos_th_GetAllListCategoryByCatalogue @catalogueid",
              new SqlParameter("catalogueid", CatalogueID)
            ).ToList();
            return data;
        }

        public IEnumerable<CategoryModel> GetSearchAllListCategoryByCatalogue(int CatalogueID, string textSearch)
        {
            var data = _context.Database.SqlQuery<CategoryModel>("pos_th_GetSearchAllListCategoryByCatalogue @catalogueid, @txtSearch",
              new SqlParameter("catalogueid", CatalogueID),
              new SqlParameter("txtSearch", textSearch)
            ).ToList();
            return data;
        }

        public int SaveMapCategoryToCatalogue(List<CategoryModel> data, int catalogueid, int userid)
        {
            try
            {
                var tabletemp = new DataTable();
                tabletemp.Columns.Add("Value");

                foreach (var item in data)
                {
                    tabletemp.Rows.Add(item.CategoryID);
                }

                var tablecategorytemp = new SqlParameter("tablecategory", SqlDbType.Structured);
                tablecategorytemp.Value = tabletemp;
                tablecategorytemp.TypeName = "dbo.TableTemp";

                var result = _context.Database.ExecuteSqlCommand("pos_th_SaveDataMapCategoryToCatalogue @catalogueid,@userid,@tablecategory",
                  new SqlParameter("catalogueid", catalogueid),
                  new SqlParameter("userid", userid),
                  tablecategorytemp
               );


                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }


        #endregion

        #region Category

        public IEnumerable<CategoryModel> GetListCategory()
        {
            var data = _context.CATEGORies.Where(x => x.Status == 1).Select(x => new CategoryModel
            {
                CategoryID = x.CategoryID,
                CategoryName = x.CategoryName,
                CategoryNameSort = x.CategoryNameSort ?? "",
                Status = x.Status,
                Color = x.Color ?? "",
                ProductColor = x.ProductColor ?? ""
            });
            return data;
        }

        public IEnumerable<CategoryModel> GetListCategory(int CurrentPage)
        {
            var data = _context.CATEGORies.Where(x => x.Status == 1).Select(x => new CategoryModel
            {
                CategoryID = x.CategoryID,
                CategoryName = x.CategoryName,
                CategoryNameSort = x.CategoryNameSort ?? "",
                Status = x.Status,
                Color = x.Color ?? "",
                ProductColor = x.ProductColor ?? ""
            }).OrderBy(p => p.CategoryName).Skip(10 * (CurrentPage - 1))
            .Take(10).ToList();
            return data;
        }

        public int GetTotalCategory()
        {
            var data = _context.CATEGORies.Where(x => x.Status == 1).Select(x => new CategoryModel
            {
                CategoryID = x.CategoryID,
                CategoryName = x.CategoryName,
                CategoryNameSort = x.CategoryNameSort ?? "",
                Status = x.Status,
                Color = x.Color ?? "",
                ProductColor = x.ProductColor ?? ""
            }).Count();
            return data;
        }

        public int SaveDataCategory(CategoryModel cate)
        {
            try
            {

                if (cate.CategoryID == 0)
                {
                    var checkdata = _context.CATEGORies.Where(x => x.CategoryName == cate.CategoryName).ToList();
                    if (checkdata.Count() > 0)
                    {
                        return -1;
                    }

                    var data = new CATEGORY();
                    data.CategoryName = cate.CategoryName;
                    data.CategoryNameSort = cate.CategoryNameSort ?? "";
                    data.Color = cate.Color ?? "";
                    data.ProductColor = cate.ProductColor ?? "";
                    data.Status = 1;
                    data.CreateBy = cate.CreateBy ?? 1;
                    data.CreateDate = DateTime.Now;
                    data.UpdateBy = cate.UpdateBy ?? 0;
                    data.UpdateDate = DateTime.Now;
                    data.Note = cate.Note ?? "";

                    _context.Entry(data).State = EntityState.Added;
                    _context.SaveChanges();

                    return 1;

                }
                else
                {
                    var checkdata = _context.CATEGORies.Where(x => x.CategoryName == cate.CategoryName && x.CategoryID != cate.CategoryID).ToList();
                    if (checkdata.Count() > 0)
                    {
                        return -1;
                    }
                    var data = _context.CATEGORies.Find(cate.CategoryID);
                    if (data != null)
                    {
                        data.CategoryName = cate.CategoryName;
                        data.CategoryNameSort = cate.CategoryNameSort ?? "";
                        data.Color = cate.Color ?? "";
                        data.ProductColor = cate.ProductColor ?? "";
                        data.Status = 1;
                        data.UpdateBy = cate.UpdateBy ?? 1;
                        data.UpdateDate = DateTime.Now;
                        data.Note = cate.Note ?? "";

                        _context.Entry(data).State = EntityState.Modified;
                        _context.SaveChanges();

                        return 1;
                    }
                    return 0;

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int RemoveCategory(int categoryid, int userid)
        {
            var data = _context.CATEGORies.Find(categoryid);
            {
                if (data != null)
                {
                    data.Status = 0;
                    data.UpdateBy = userid;
                    data.UpdateDate = DateTime.Now;
                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return 1;
                }
            }
            return 0;
        }

        public IEnumerable<ProductionModel> GetProductByCategoryID(int CategoryID, int CurrentPage)
        {

            var data = _context.MAP_PRODUCT_TO_CATEGORY.Join(_context.PRODUCTs, cate => cate.ProductID,
                pro => pro.ProductID, (cate, pro) => new { cate, pro })
                .Where(x => x.cate.CategoryID == CategoryID && x.cate.Status == 1)
                .Select(x => new ProductionModel()
                {
                    ProductID = x.cate.ProductID,
                    CategoryID = x.cate.CategoryID,
                    ProductNameDesc = x.pro.ProductNameDesc,
                    ProductNameSort = x.pro.ProductNameSort,
                    Status = x.pro.Status

                }).OrderBy(p => p.ProductNameDesc).Skip(10 * (CurrentPage - 1))
            .Take(10).ToList(); 
            return data;
        }

        public IEnumerable<ProductionModel> GetProductByCategoryID(int CategoryID)
        {

            var data = _context.MAP_PRODUCT_TO_CATEGORY.Join(_context.PRODUCTs, cate => cate.ProductID,
                pro => pro.ProductID, (cate, pro) => new { cate, pro })
                .Where(x => x.cate.CategoryID == CategoryID && x.cate.Status == 1)
                .Select(x => new ProductionModel()
                {
                    ProductID = x.cate.ProductID,
                    CategoryID = x.cate.CategoryID,
                    ProductNameDesc = x.pro.ProductNameDesc,
                    ProductNameSort = x.pro.ProductNameSort,
                    Status = x.pro.Status

                });

            return data;
        }

        public IEnumerable<ProductionModel> GetAllListProductByCategory(int CategoryID)
        {
            var data = _context.Database.SqlQuery<ProductionModel>("pos_th_GetAllListProductByCategory @categoryid",
              new SqlParameter("categoryid", CategoryID)
            ).ToList();
            return data;
        }

        public IEnumerable<ProductionModel> GetSearchAllListProductByCategory(int CategoryID, string textSearch)
        {
            var data = _context.Database.SqlQuery<ProductionModel>("pos_th_GetSearchAllListProductByCategory @categoryid, @txtSearch",
              new SqlParameter("categoryid", CategoryID),
              new SqlParameter("txtSearch", textSearch)
            ).ToList();
            return data;
        }


        public int SaveDataMapProductToCategory(List<ProductionModel> data, int categoryid, int userid)
        {
            try
            {
                var tabletemp = new DataTable();
                tabletemp.Columns.Add("Value");

                foreach (var item in data)
                {
                    tabletemp.Rows.Add(item.ProductID);
                }

                var tableproducttemp = new SqlParameter("tableproduct", SqlDbType.Structured);
                tableproducttemp.Value = tabletemp;
                tableproducttemp.TypeName = "dbo.TableTemp";

                var result = _context.Database.ExecuteSqlCommand("pos_th_SaveDataMapProductToCategory @categoryid,@userid,@tableproduct",
                  new SqlParameter("categoryid", categoryid),
                  new SqlParameter("userid", userid),
                  tableproducttemp
               );


                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public IEnumerable<CategoryModel> searchProduct(string textSearch, int type)
        {
            var data = _context.Database.SqlQuery<CategoryModel>("pos_th_SearchProduct @txtSearch, @type", new SqlParameter("txtSearch", textSearch), new SqlParameter("type", type)).ToList();
            return data;
        }

        #endregion

        #endregion


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
    }
}

