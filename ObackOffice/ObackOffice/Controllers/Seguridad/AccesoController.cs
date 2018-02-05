using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObackOffice.Models;
using ObackOffice.Utils;
using Newtonsoft.Json;

namespace ObackOffice.Controllers.Seguridad
{
    public class AccesoController : Controller
    {
        public ActionResult Index()
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            ViewBag.Genero = API.Get<List<Parametro>>("Person/GetGeneros");
            ViewBag.Roles = API.Get<List<Parametro>>("Person/GetRoles");
            return View("CrearPersona");
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
    }
}