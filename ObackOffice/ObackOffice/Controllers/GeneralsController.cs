using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObackOffice.Controllers
{
    public class GeneralsController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoute("General_login");
        }
        
        public ActionResult Logout()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("~/Views/Generals/Login.cshtml");
        }
    }
}