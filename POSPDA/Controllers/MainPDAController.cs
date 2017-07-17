using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelPOS;
using ServicePOS.Model;
using ServicePOS;

namespace POSPDA.Controllers
{
    public class MainPDAController : Controller
    {
        //
        // GET: /MainPDA/
        private IOrderService _orderService;
        private IOrderService OrderService
        {
            get { return _orderService ?? (_orderService = new OrderService()); }
            set { _orderService = value; }
        }

        private IUserService _userservice;
        private IUserService UserService
        {
            get { return _userservice ?? (_userservice = new UserService()); }
            set { _userservice = value; }
        }

        public ActionResult Index()
        {
            var lst = OrderService.GetListStatusTable("");
            ViewBag.listOrder = lst;
            return View();
        }

        public ActionResult CheckLogin(string Password)
        {
            try
            {
                StaffModel item = null;
                int result = 0;
                var UserGet = UserService.GetStaffByUserName(Class.UserName);
                foreach (StaffModel _item in UserGet)
                {
                    item = new StaffModel();
                    item.UserName = _item.UserName;
                    item.Password = _item.Password;
                }
                string PassCompare = StaffModel.Decrypt(item.Password);
                if (Password == PassCompare)
                {
                    result = 1;
                    this.Session["User"] = item;
                }
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetStatusTable(string tableID)
        {
            try
            {
                if (Session["User"] != null)
                {
                    var lstStatus = OrderService.GetListStatusTable(tableID);
                    return Json(lstStatus, JsonRequestBehavior.AllowGet);
                }
                else {
                    return RedirectToAction("Index", "Loging");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewOrder(string TableID)
        {
            Class.FloorID = TableID;
            return Json(Class.FloorID, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut()
        {
            Class.UserName = null;
            Class.FloorID = null;
            this.Session["User"] = null;
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        
    }
}
