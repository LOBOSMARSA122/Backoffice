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
            return View();
        }

        public ActionResult GetAccordion(string data)
        {
            ViewBag.Accordion = data;
            return PartialView("_AccordionPartial");
        }

        public JsonResult GetTreeData(int data)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("id", data.ToString());
            List<TreeView> Tree = API.Get<List<TreeView>>("Perfiles/GetTreeView", args);
            return Json(Tree);
        }

        public JsonResult AddNewGender(string input)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Valor1", input }
            };
            Parametro response = API.Post<Parametro>("Person/InsertGenero", args);
            return Json(response);
        }

        public JsonResult AddNewRol(string input, string tree)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Nombre", input },
                { "Tree", tree}
            };
            Parametro response = API.Post<Parametro>("Perfiles/InsertRol", args);
            return Json(response);
        }
    }
}