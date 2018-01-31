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
            Api API = new Api();
            string url = "Usuario/GetAutorizacion";
            Dictionary<string, string> AccesoUsuario = new Dictionary<string, string>();
            AccesoUsuario.Add("rolId", "1");
            ViewBag.MENU = API.Get<List<Models.Acceso.Autorizacion>>(url, AccesoUsuario);
            return View("~/Views/Generals/Index.cshtml",ViewBag.MENU);
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
                ViewBag.USUARIO = API.Get<Models.Acceso.Usuario>(url, AccesoUsuario);
                if (ViewBag.USUARIO != null)
                {
                    return RedirectToRoute("backoffice");
                }
                else
                {
                    return RedirectToRoute("General_NotAuthorized");
                }
                //if (((List<Models.Acceso.Autorizacion>)ViewBag.RESPONSE).ToList().Count > 0)

            }
            return RedirectToRoute("General_NotAuthorized");
        }

        public ActionResult Autorizacion(int rolId)
        {

            return null;
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