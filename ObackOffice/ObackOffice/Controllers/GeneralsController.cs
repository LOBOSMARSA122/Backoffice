using ObackOffice.Controllers.Seguridad;
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

        [GeneralSecurity(Rol = "")]
        public ActionResult Home()
        {
            return View("~/Views/Generals/Index.cshtml",ViewBag.MENU);
        }

        public ActionResult Logout()
        {
            Session.Remove("AutBackoffice");           
            Session.RemoveAll();    
            return RedirectToRoute("General_login");
        }

        public ActionResult Login()
        {
            return View("~/Views/Generals/Login.cshtml");
        }

        public ActionResult Register()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];
            return View();
        }

        #region Authentication

        public ActionResult Login_authentication(FormCollection collection)
        {
            if (TempData["FormCollection"] != null)
                collection = (FormCollection)TempData["FormCollection"];

            if (collection.Get("usuario").Trim() != string.Empty && collection.Get("pass").Trim() != string.Empty)
            {
                TempData["FormCollection"] = null;
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
                    oclientSession.foto = ViewBag.USUARIO.foto;
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
            return RedirectToAction("backoffice");
        }

        public ActionResult SessionExpired()
        {
            Session.Remove("AutBackoffice");
            Session.RemoveAll();
            return RedirectToRoute("General_login");
        }

        public ActionResult VerifyRegister(FormCollection collection)
        {
            if(string.IsNullOrWhiteSpace(collection.Get("usuario")) || string.IsNullOrWhiteSpace(collection.Get("email")) || string.IsNullOrWhiteSpace(collection.Get("cargo")) || string.IsNullOrWhiteSpace(collection.Get("pass")) || string.IsNullOrWhiteSpace(collection.Get("repass")))
            {
                TempData["Message"] = "Debe completar todos los campos marcados con un (*)";
                return RedirectToAction("Register");
            }

            if (string.IsNullOrWhiteSpace(collection.Get("terminos")))
            {
                TempData["Message"] = "Debe aceptar los términos y condiciones";
                return RedirectToAction("Register");
            }

            if(collection.Get("pass") != collection.Get("pass"))
            {
                TempData["Message"] = "Las contraseñas deben de coincidir";
                return RedirectToAction("Register");
            }

            Api API = new Api();

            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                { "usuario", collection.Get("usuario") },
                { "email", collection.Get("email")},
                { "cargo" ,collection.Get("cargo")},
                { "pass",collection.Get("pass") },
                { "telefono" , collection.Get("telefono")}
            };
            string Message = API.Get<string>("Empleado/VerificaYRegistraEmpleado",args);

            if(Message != "Ok")
            {
                TempData["Message"] = Message;
                return RedirectToAction("Register");
            }
            else
            {
                TempData["FormCollection"] = collection;
            }

            return RedirectToAction("Login_authentication");
        }

        #endregion
    }
}