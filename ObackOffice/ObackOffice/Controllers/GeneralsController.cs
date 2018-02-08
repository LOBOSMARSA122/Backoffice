using ObackOffice.Models;
using ObackOffice.Utils;
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
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            return View("~/Views/Generals/Index.cshtml",ViewBag.MENU);
        }

        public ActionResult Logout()
        {
            Session.Remove("AutBackoffice");           
            Session.RemoveAll();    
            return RedirectToRoute("General_sessionexpired");
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
                ViewBag.USUARIO = API.Get<Models.Acceso.UsuarioLogin>(url, AccesoUsuario);
                if (ViewBag.USUARIO != null)
                {
                    ClientSession oclientSession = new ClientSession();
                    oclientSession.UsuarioId = ViewBag.USUARIO.UsuarioId;
                    oclientSession.PersonaId = ViewBag.USUARIO.PersonaId;
                    oclientSession.EmpresaId = ViewBag.USUARIO.EmpresaId;
                    oclientSession.NombreUsuario = ViewBag.USUARIO.NombreUsuario;
                    oclientSession.NombreCompleto = ViewBag.USUARIO.NombreCompleto;
                    oclientSession.FechaCaduca = ViewBag.USUARIO.FechaCaduca;
                    oclientSession.RolId = ViewBag.USUARIO.RolId;
                    oclientSession.Rol = ViewBag.USUARIO.Rol;
                    oclientSession.Rol = oclientSession.Rol.Substring(0, 3);
                    oclientSession.Autorizacion = ViewBag.USUARIO.Autorizacion;
                    Session.Add("AutBackoffice", oclientSession);
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

        public ActionResult SessionExpired()
        {
            Session.Remove("AutBackoffice");
            Session.RemoveAll();
            return View();
        }

        #endregion
    }
}