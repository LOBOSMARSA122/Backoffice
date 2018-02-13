using ObackOffice.Models;
using ObackOffice.Models.Comun;
using Newtonsoft.Json;
using ObackOffice.Utils;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Reportes
{
    public class ReporteMultipleController : Controller
    {
        public JsonResult Chart(string[] Data, string Action)
        {
            Api API = new Api();
            List<ReporteMultipleList> Listado = new List<ReporteMultipleList>();

            foreach (string D in Data)
            {
                Listado.Add(new ReporteMultipleList()
                {
                    EmpleadoCursoId = int.Parse(D.Split('-')[0]),
                    PersonaId = int.Parse(D.Split('-')[1])
                });
            }
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", Action },
                { "String2", JsonConvert.SerializeObject(Listado) }
            };

            string base64 = API.Post<string>("ReporteMultiple/Chart", args);

            return Json(base64);
        }

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

        public ActionResult FiltrarReporteMultiple(string SedeId, string EventoId, string CursoId, string NombreEmpleado, string DNIEmpleado, string Index, string Take)
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
            ViewBag.REGISTROS = API.Post<BandejaReporteMultiple>("ReporteMultiple/BandejaReporteMultiple", args);
            return PartialView("_ReporteMultiplePartial");
        }

        public JsonResult CrearExcel(int SedeId, int EventoId, int CursoId, string NombreEmpleado, string DNIEmpleado, string[] Lista, string[] Charts, int Index, int Take)
        {
            Api API = new Api();

            List<ReporteMultipleList> List = new List<ReporteMultipleList>();

            foreach (string L in Lista)
            {
                List.Add(new ReporteMultipleList()
                {
                    EmpleadoCursoId = int.Parse(L.Split('-')[0]),
                    PersonaId = int.Parse(L.Split('-')[1])
                });
            }
            BandejaReporteMultiple data = new BandejaReporteMultiple()
            {
                Index = Index,
                Take = Take,
                Charts = Charts,
                Lista = List,
                SedeId = SedeId,
                EventoId = EventoId,
                CursoId = CursoId,
                NombreEmpleado = NombreEmpleado,
                DNIEmpleado = DNIEmpleado
            };

            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", JsonConvert.SerializeObject(data) }
            };

            byte[] ms = API.PostDownloadStream("ReporteMultiple/BandejaReporteMultipleExcel", args);

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