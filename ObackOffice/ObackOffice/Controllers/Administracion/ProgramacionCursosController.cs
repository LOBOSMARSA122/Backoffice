using ObackOffice.Models;
using ObackOffice.Models.Comun;
using ObackOffice.Models.Administracion;
using Newtonsoft.Json;
using ObackOffice.Utils;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Administracion
{
    public class ProgramacionCursosController : Controller
    {
        public ActionResult Index()
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Sedes).ToString() }
            };
            ViewBag.SEDES = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.All);

            return View();
        }

        public JsonResult FiltrarCalendario(string SedeId, string EventoId, string CursoId, string year, string month)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "SedeId",SedeId },
                { "EventoId",EventoId },
                { "CursoId",CursoId },
                { "year",year },
                { "month",month }
            };

            List<Agenda> Result = API.Get<List<Agenda>>("ProgramacionCursos/FiltrarCalendario", args);

            return Json(Result);
        }
    }
}