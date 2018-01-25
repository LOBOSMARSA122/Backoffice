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

        public ActionResult Home()
        {
            return View("~/Views/Generals/Index.cshtml");
        }

        public ActionResult Logout()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("~/Views/Generals/Login.cshtml");
        }

        public ActionResult backoffice()
        {
            return View("~/Views/Person/Index.cshtml");
        }

        #region Authentication

        public ActionResult Login_authentication(FormCollection collection)
        {
            if (collection.Get("usuario").Trim() != "")
            {
                //Validar si el usuario existe en el sistema

                return RedirectToRoute("backoffice");
            }
            return RedirectToRoute("General_NotAuthorized");
        }

        #endregion
    }
}