using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelPOS;
using ServicePOS.Model;
using ServicePOS;
using SystemLog;

namespace POSPDA.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/
        
        private  ICatalogueService _categoryService;
        private ICatalogueService CategoryService
        {
            get { return _categoryService ?? (_categoryService = new CatalogueService()); }
            set { _categoryService = value; }
        }
        private IProductService _productService;
        private IProductService ProductService
        {
            get { return _productService ?? (_productService = new ProductService()); }
            set { _productService = value; }
        }
        private IModifireService _modifireService;
        private IModifireService ModifireService
        {
            get { return _modifireService ?? (_modifireService = new ModifireService()); }
            set { _modifireService = value; }
        }
        private IPrinterService _printService;
        private IPrinterService PrintService
        {
            get { return _printService ?? (_printService = new PrinterService()); }
            set { _printService = value; }
        }

        public ActionResult LoadMenu(String ID)
        {
            //Class.FloorID = ID;
            var data = PrintService.GetListPrinterNotPayment();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (PrinterModel item in data)
            {
                SelectListItem s = new SelectListItem();
                s.Text = item.PrintName;
                s.Value = item.ID.ToString();
                lst.Add(s);
            }
            ViewData["Printer"] = lst;
            return View();
        }

        public ActionResult Index(String ID)
        {
            Class.FloorID = ID;
            
            return RedirectToAction("LoadMenu");
        }
        public ActionResult LoadMenuOf(int ID)
        {
            try
            {
                var list = CategoryService.GetCategoryByCatalogueID(ID).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SystemLog.LogPOS.WriteLog("MenuController::::::::::::::::::::LoadMenuOf::::::::::::::::::" + ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LoadProduct(int ID)
        {
            try
            {
                var lstProduct = ProductService.GetProdutcByCategory(ID, 1);
                return Json(lstProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SystemLog.LogPOS.WriteLog("MenuController::::::::::::::::::::LoadProduct::::::::::::::::::" + ex.Message);
                return null;
            }
        }
        public ActionResult LoadModifier(int ID)
        {
            try {
               
                var listModifier = ModifireService.GetModifireByProduct(ID, 1);
                return Json(listModifier, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SystemLog.LogPOS.WriteLog("MenuController::::::::::::::::::::LoadModifier::::::::::::::::::" + ex.Message);
                return null;
            }
        }

        public ActionResult LoadPrinter()
        {
            try
            {
                var data = PrintService.GetListPrinterNotPayment();
                List<PrinterModel> lst = new List<PrinterModel>();
                foreach (PrinterModel item in data)
                {
                    lst.Add(item);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SystemLog.LogPOS.WriteLog("frmOpenItem::::::::::::::::::::::::LoadPrinter::::::::::::::::::" + ex.Message);
            } return null;

        }

        public ActionResult LoadCatalogue()
        {
            try
            {
                var data = CategoryService.GetCatalogueList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
