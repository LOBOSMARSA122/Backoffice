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
        public JsonResult Chart(string Condicion, string Asistencia, string CursoId, string NombreEmpleado, string DNIEmpleado, string Action)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            { 
                { "SedeId", "1" },
                { "Condicion", Condicion },
                { "Asistencia", Asistencia},
                { "EventoId", "1" },
                { "CursoId", CursoId },
                { "NombreEmpleado", NombreEmpleado },
                { "DNIEmpleado", DNIEmpleado },
                { "Action", Action }
            };

            string base64 = API.Get<string>("ReporteMultiple/Chart", args);

            return Json(base64);
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

            ViewBag.REGISTROS = new BandejaReporteMultiple() { Lista = new List<ReporteMultipleList>(),Take = 10};
            return View();
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Múltiple")]
        public ActionResult FiltrarReporteMultiple(string Condicion, string Asistencia, string CursoId, string NombreEmpleado, string DNIEmpleado, string Index, string Take)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "SedeId", "1" },
                { "Condicion", Condicion },
                { "Asistencia", Asistencia },
                { "EventoId", "1" },
                { "CursoId", CursoId },
                { "NombreEmpleado", NombreEmpleado },
                { "DNIEmpleado", DNIEmpleado },
                { "Index", Index },
                { "Take", Take }
            };
            ViewBag.REGISTROS = API.Post<BandejaReporteMultiple>("ReporteMultiple/BandejaReporteMultiple", args);
            return PartialView("_ReporteMultiplePartial");
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Múltiple")]
        public JsonResult CrearExcel(int Condicion, int Asistencia, int CursoId, string NombreEmpleado, string DNIEmpleado, string[] Charts)
        {
            Api API = new Api();

            BandejaReporteMultiple data = new BandejaReporteMultiple()
            {
                Index = 1,
                Take = 0,
                Charts = Charts,
                SedeId = 1,
                EventoId = 1,
                CursoId = CursoId,
                NombreEmpleado = NombreEmpleado,
                DNIEmpleado = DNIEmpleado,
                Condicion = Condicion,
                Asistencia = Asistencia
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