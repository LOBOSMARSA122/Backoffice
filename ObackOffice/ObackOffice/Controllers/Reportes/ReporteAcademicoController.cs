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
                { "Take", "10" }
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

        public ActionResult FiltrarDataBandeja()
        {
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "SedeId", "0" },
                { "EventoId", "0" },
                { "CursoId", "0" },
                { "NombreEmpleado", "0" },
                { "SedeId", "0" },
                { "Index", "1" },
                { "Take", "10" }
            };
            return PartialView();
        }
    }
}