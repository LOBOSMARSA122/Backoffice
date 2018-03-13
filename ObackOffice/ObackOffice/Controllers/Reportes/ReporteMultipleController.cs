using ObackOffice.Controllers.Seguridad;
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
        [GeneralSecurity(Rol = "Reportes-Reporte Múltiple")]
        public JsonResult Chart(string area, string categoria, string empresa, string capacitador, string sede, string Condicion, string Asistencia, string CursoId, string NombreEmpleado, string Action, string FechaInicio, string FechaFin)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Area", area },
                { "Categoria", categoria },
                { "Empresa", empresa },
                { "Capacitador", capacitador },
                { "SedeId", sede },
                { "Condicion", Condicion },
                { "Asistencia", Asistencia},
                { "EventoId", "1" },
                { "CursoId", CursoId },
                { "NombreEmpleado", NombreEmpleado },
                { "FechaInicio", FechaInicio},
                { "FechaFin", FechaFin},
                { "Action", Action }
            };

            string JsonString = API.Get<string>("ReporteMultiple/Chart", args);

            return Json(JsonString);
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Múltiple")]
        public ActionResult Index()
        {
            Api API = new Api();

            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Condicion).ToString() }
            };
            ViewBag.CONDICION = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.All);

            args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Asistencia).ToString() }
            };
            ViewBag.ASISTENCIA = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.All);

            args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Sedes).ToString() }
            };
            ViewBag.SEDES = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.All);

            ViewBag.CURSOS = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Curso/ddlCurso"), Constantes.All);

            ViewBag.CAPACITADORES = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Capacitador/ddlCapacitador"), Constantes.All);

            ViewBag.REGISTROS = new BandejaReporteMultiple() { Lista = new List<ReporteMultipleList>(),Take = 10};
            return View();
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Múltiple")]
        public ActionResult FiltrarReporteMultiple(string area, string categoria, string empresa, string capacitador, string sede, string Condicion, string Asistencia, string CursoId, string NombreEmpleado, string Ranking, string FechaInicio, string FechaFin, string Index, string Take)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Area", area },
                { "Categoria", categoria },
                { "Empresa", empresa},
                { "CapacitadorId", capacitador },
                { "SedeId", sede },
                { "Condicion", Condicion },
                { "Asistencia", Asistencia },
                { "EventoId", "1" },
                { "CursoId", CursoId },
                { "NombreEmpleado", NombreEmpleado },
                { "Ranking", Ranking },
                { "FechaInicio", FechaInicio },
                { "FechaFin", FechaFin },
                { "Index", Index },
                { "Take", Take }
            };
            ViewBag.REGISTROS = API.Post<BandejaReporteMultiple>("ReporteMultiple/BandejaReporteMultiple", args);
            return PartialView("_ReporteMultiplePartial");
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Múltiple")]
        public JsonResult CrearExcel(string area, string categoria, string empresa, int capacitador, int sede, int Condicion, int Asistencia, int CursoId, string NombreEmpleado, string FechaInicio, string FechaFin, string[] Charts)
        {
            Api API = new Api();

            BandejaReporteMultiple data = new BandejaReporteMultiple()
            {
                Area = area,
                Categoria = categoria,
                Empresa = empresa,
                CapacitadorId = capacitador,
                Index = 1,
                Take = 0,
                Charts = Charts,
                SedeId = sede,
                EventoId = 1,
                CursoId = CursoId,
                NombreEmpleado = NombreEmpleado,
                Condicion = Condicion,
                Asistencia = Asistencia,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin
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

        public JsonResult Autocomplete(string campo, string valor)
        {
            Api API = new Api();
            string url = "ReporteMultiple/GetAutocomplete";
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "campo",campo},
                { "valor",valor }
            };
            List<string> data = API.Get<List<string>>(url, args);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}