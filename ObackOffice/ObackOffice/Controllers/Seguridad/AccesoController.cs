using System.Collections.Generic;
using System.Web.Mvc;
using ObackOffice.Models;
using ObackOffice.Utils;
using ObackOffice.Models.Comun;

namespace ObackOffice.Controllers.Seguridad
{
    public class AccesoController : Controller
    {
        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public ActionResult BandejaUsuarios()
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Index","1" },
                { "Take","10"}
            };
            ViewBag.Usuarios = API.Post<BandejaUsuario>("Usuario/GetUsuarios", arg);
            return View();
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
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

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public ActionResult CrearUsuario(int? id)
        {
            Api API = new Api();

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

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public ActionResult GetAccordion(string data)
        {
            ViewBag.Accordion = data;
            return PartialView("_AccordionPartial");
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public JsonResult GetTreeData(int data)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("id", data.ToString());
            List<TreeView> Tree = API.Get<List<TreeView>>("Perfiles/GetTreeView", args);
            return Json(Tree);
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public JsonResult AddNewGender(string input)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", input },
                { "Int1", ViewBag.USUARIO.UsuarioId.ToString() }
            };
            Parametro response = API.Post<Parametro>("Person/InsertGenero", args);
            return Json(response);
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public JsonResult AddNewRol(string input, string tree)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", input },
                { "String2", tree},
                { "Int1", ViewBag.USUARIO.UsuarioId.ToString() }
            };
            Parametro response = API.Post<Parametro>("Perfiles/InsertRol", args);
            return Json(response);
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public JsonResult InsertNewPerson(string Persona, string Usuario)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", Persona },
                { "String2", Usuario },
                { "Int1", ViewBag.USUARIO.UsuarioId.ToString() }
            };
            bool response = API.Post<bool>("Person/InsertNewPerson", args);
            return Json(response);
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public JsonResult EditPerson(string Persona, string Usuario)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", Persona },
                { "String2", Usuario },
                { "Int1", ViewBag.USUARIO.UsuarioId.ToString() }
            };
            bool response = API.Post<bool>("Person/EditPerson", args);
            return Json(response);
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public JsonResult DeleteUser(int id)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Int1", id.ToString() },
                { "Int2", ViewBag.USUARIO.UsuarioId.ToString() }
            };
            bool response = API.Post<bool>("Usuario/DeleteUser", args);
            return Json(response);
        }

        [GeneralSecurity(Rol = "Seguridad-Usuario de Sistema")]
        public JsonResult CrearExcel(BandejaUsuario data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "NombrePersona",data.NombrePersona },
                { "NombreUsuario", data.NombreUsuario},
                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };
            byte[] ms = API.PostDownloadStream("Usuario/BandejaUsuarioExcel", arg);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Probando.xlsx");
            Response.BinaryWrite(ms);
            Response.End();

            return Json(Response);
        }
    }
}