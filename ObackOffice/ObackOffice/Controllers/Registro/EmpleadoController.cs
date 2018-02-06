using ObackOffice.Models;
using ObackOffice.Models.Comun;
using ObackOffice.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Registro
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Agenda()
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "grupoId", "100" },
                { "accion","" },
            };
            ViewBag.Roles = API.Get<List<Parametro>>("Parametr/GetParametroByGrupoId", args);
            return View();
        }
    }
}