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
    public class FloorController : Controller
    {
        //
        // GET: /Floor/
        private IOrderService _orderService;
        private IOrderService OrderService
        {
            get { return _orderService ?? (_orderService = new OrderService()); }
            set { _orderService = value; }
        }

        public ActionResult Floor()
        {
            return View();
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
                else
                {
                    return Json("NULL", JsonRequestBehavior.AllowGet);
                }
                
                

            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
