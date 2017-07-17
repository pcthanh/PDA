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
    public class LoginController : Controller
    {

        private IUserService _userservice;
        private IUserService UserService
        {
            get { return _userservice ?? (_userservice = new UserService()); }
            set { _userservice = value; }
        }
        //
        // GET: /Login/

        public ActionResult Index()
        {
            var lst = UserService.GetListStaff();
            ViewBag.listUser = lst;
            return View();
        }

        public ActionResult LoginAction(string username)
        {
            Class.UserName = username;
            ViewBag.UserName = Class.UserName;
            return View();
        }

    }
}
