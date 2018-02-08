using ObackOffice.Models;
using ObackOffice.Models.Comun;
using ObackOffice.Utils;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Reportes
{
    public class ReporteAcademicoController : Controller
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
            args = new Dictionary<string, string>
            {
                { "Index", "1" },
                { "Take", "10" },
                { "SedeId", "-1" },
                { "EventoId", "-1" },
                { "CursoId", "-1" }
            };
            ViewBag.REGISTROS = API.Post<BandejaReporteAcademico>("ReporteAcademico/BandejaReporteAcademico", args);
            return View("ReporteAcademico");
        }

        public JsonResult GetComboData(string combo, string valor)
        {
            Api API = new Api();
            List<Dropdownlist> response = null;
            switch (combo)
            {
                case "sede":
                    {
                        Dictionary<string, string> args = new Dictionary<string, string>
                        {
                            { "sedeId", valor }
                        };
                        response = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Eventos/ddlEventos", args), Constantes.All);
                        break;
                    }
                case "evento":
                    {
                        Dictionary<string, string> args = new Dictionary<string, string>
                        {
                            { "eventoId", valor }
                        };
                        response = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("CursoProgramado/ddlCursoProgramdos", args), Constantes.All);
                        break;
                    }
            }
            
            return Json(response);
        }

        public ActionResult FiltrarDataBandeja(string SedeId, string EventoId, string CursoId, string NombreEmpleado, string DNIEmpleado, string Index, string Take)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "SedeId", SedeId },
                { "EventoId", EventoId },
                { "CursoId", CursoId },
                { "NombreEmpleado", NombreEmpleado },
                { "DNIEmpleado", DNIEmpleado },
                { "Index", Index },
                { "Take", Take }
            };
            ViewBag.REGISTROS = API.Post<BandejaReporteAcademico>("ReporteAcademico/BandejaReporteAcademico", args);
            return PartialView("_ReporteAcademicoPartial");
        }

        public ActionResult DetalleEmpleado(string PersonaId, string cursoProgramadoId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "PersonaId", PersonaId },
                { "cursoProgramadoId", cursoProgramadoId }
            };
            ViewBag.DETALLE = API.Get<List<ReporteAcademicoListClase>>("ReporteAcademico/DetalleEmpleado", args);
            return PartialView("_ReporteAcademicoDetallePartial");
        }
    }
}