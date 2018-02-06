using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObackOffice.Models;
using ObackOffice.Utils;
using Newtonsoft.Json;
using ObackOffice.Models.Comun;

namespace ObackOffice.Controllers.Seguridad
{
    public class AccesoController : Controller
    {
        public ActionResult BandejaUsuarios()
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Index","1" },
                { "Take","10"}
            };
            ViewBag.Usuarios = API.Post<BandejaUsuario>("Usuario/GetUsuarios", arg);
            return View();
        }

        public ActionResult FiltrarUsuario(BandejaUsuario data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "NombrePersona",data.NombrePersona },
                { "NombreUsuario", data.NombreUsuario},
                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };
            ViewBag.Usuarios = API.Post<BandejaUsuario>("Usuario/GetUsuarios",arg);
            return PartialView("_BandejaUsuariosPartial");
        }

        public ActionResult CrearUsuario(int? id)
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            ViewBag.Genero = API.Get<List<Parametro>>("Person/GetGeneros");
            ViewBag.Roles = API.Get<List<Parametro>>("Person/GetRoles");
            ViewBag.TipoDocumento = API.Get<List<Parametro>>("Person/GetTipoDocumentos");
            ViewBag.Empresas = API.Get<List<Models.Administracion.Empresa>>("Empresas/GetEmpresas");
            if (id.HasValue)
            {
                ViewBag.EditUser = API.Get<Models.Acceso.Usuario>("Usuario/GetUsuario", new Dictionary<string, string> { { "id", id.Value.ToString() } });
                ViewBag.EditPerson = API.Get<Models.Comun.Persona>("Person/GetPersona", new Dictionary<string, string> { { "id", ViewBag.EditUser.PersonaId.ToString() } });
            }
                
            return View();
        }

        public ActionResult GetAccordion(string data)
        {
            if (((ClientSession)Session["AutBackoffice"]) == null)
                return null;

            ViewBag.Accordion = data;
            return PartialView("_AccordionPartial");
        }

        public JsonResult GetTreeData(int data)
        {
            if (((ClientSession)Session["AutBackoffice"]) == null)
                return null;

            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("id", data.ToString());
            List<TreeView> Tree = API.Get<List<TreeView>>("Perfiles/GetTreeView", args);
            return Json(Tree);
        }

        public JsonResult AddNewGender(string input)
        {
            if (((ClientSession)Session["AutBackoffice"]) == null)
                return null;

            ClientSession Usuario = ((ClientSession)Session["AutBackoffice"]);

            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", input },
                { "Int1", Usuario.UsuarioId.ToString() }
            };
            Parametro response = API.Post<Parametro>("Person/InsertGenero", args);
            return Json(response);
        }

        public JsonResult AddNewRol(string input, string tree)
        {
            if (((ClientSession)Session["AutBackoffice"]) == null)
                return null;

            ClientSession Usuario = ((ClientSession)Session["AutBackoffice"]);

            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", input },
                { "String2", tree},
                { "Int1", Usuario.UsuarioId.ToString() }
            };
            Parametro response = API.Post<Parametro>("Perfiles/InsertRol", args);
            return Json(response);
        }

        public JsonResult InsertNewPerson(string Persona, string Usuario)
        {
            if (((ClientSession)Session["AutBackoffice"]) == null)
                return null;

            ClientSession User = ((ClientSession)Session["AutBackoffice"]);

            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", Persona },
                { "String2", Usuario },
                { "Int1", User.UsuarioId.ToString() }
            };
            bool response = API.Post<bool>("Person/InsertNewPerson", args);
            return Json(response);
        }

        public JsonResult EditPerson(string Persona, string Usuario)
        {
            if (((ClientSession)Session["AutBackoffice"]) == null)
                return null;

            ClientSession User = ((ClientSession)Session["AutBackoffice"]);

            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", Persona },
                { "String2", Usuario },
                { "Int1", User.UsuarioId.ToString() }
            };
            bool response = API.Post<bool>("Person/EditPerson", args);
            return Json(response);
        }

        public JsonResult DeleteUser(int id)
        {
            if (((ClientSession)Session["AutBackoffice"]) == null)
                return null;

            ClientSession User = ((ClientSession)Session["AutBackoffice"]);

            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Int1", id.ToString() },
                { "Int2", User.UsuarioId.ToString() }
            };
            bool response = API.Post<bool>("Usuario/DeleteUser", args);
            return Json(response);
        }
    }
}