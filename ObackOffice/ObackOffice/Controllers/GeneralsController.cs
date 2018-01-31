using ObackOffice.Models;
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
            if (collection.Get("usuario").Trim() != string.Empty && collection.Get("pass").Trim() != string.Empty)
            {
                string usuario = collection.Get("usuario").Trim();
                string contrasenia = Utils.Utils.Encrypt(collection.Get("pass").Trim());
                //Validar si el usuario existe en el sistema
                Api API = new Api();
                string url = "Usuario/GetUsuario";

                Dictionary<string, string> AccesoUsuario = new Dictionary<string, string>();
                AccesoUsuario.Add("usuario", usuario);
                AccesoUsuario.Add("contrasenia", contrasenia);
                ViewBag.RESPONSE = API.Get<Models.Acceso.Usuario>(url, AccesoUsuario);

                if (ViewBag.RESPONSE != null)
                {
                    return RedirectToRoute("backoffice");
                }
                else
                {
                    return RedirectToRoute("General_NotAuthorized");
                }               
            }
            return RedirectToRoute("General_NotAuthorized");
        }

        public ActionResult Notauthorized()
        {
            Session.Remove("Auth");
            Session.Remove("AuthBackoffice");
            Session.Remove("AuthPArent");
            Session.RemoveAll();
            return View();
        }

        #endregion
    }
}