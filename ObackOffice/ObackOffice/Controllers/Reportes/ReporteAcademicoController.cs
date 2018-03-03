using ObackOffice.Controllers.Seguridad;
using ObackOffice.Models;
using ObackOffice.Models.Comun;
using ObackOffice.Models.Cliente;
using ObackOffice.Utils;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Reportes
{
    public class ReporteAcademicoController : Controller
    {
        [GeneralSecurity(Rol = "Reportes-Reporte Académico")]
        public ActionResult Index()
        {
            Api API = new Api();

            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Condicion).ToString() }
            };
            ViewBag.CONDICION = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.All);

            ViewBag.CURSOS = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Curso/ddlCurso"), Constantes.All);

            ViewBag.REGISTROS = new BandejaReporteAcademico() { Lista = new List<ReporteAcademicoList>(), Take = 10};
            return View("ReporteAcademico");
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Académico")]
        public ActionResult FiltrarDataBandeja(string Condicion, string CursoId, string NombreEmpleado, string DNIEmpleado, string Index, string Take)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "SedeId", "1" },
                { "EventoId", "1" },
                { "CursoId", CursoId },
                { "Condicion" , Condicion},
                { "NombreEmpleado", NombreEmpleado },
                { "DNIEmpleado", DNIEmpleado },
                { "Index", Index },
                { "Take", Take }
            };
            ViewBag.REGISTROS = API.Post<BandejaReporteAcademico>("ReporteAcademico/BandejaReporteAcademico", args);
            return PartialView("_ReporteAcademicoPartial");
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Académico")]
        public ActionResult DetalleEmpleado(string PersonaId, string cursoProgramadoId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "PersonaId", PersonaId },
                { "cursoProgramadoId", cursoProgramadoId }
            };
            ViewBag.DETALLE = API.Get<List<ReporteAcademicoListClase>>("ReporteAcademico/DetalleEmpleado", args);
            ViewBag.Talleres = API.Get<List<EmpleadoTaller>>("ReporteAcademico/TallerEmpleado", args);
            args = new Dictionary<string, string>
            {
                { "grupoId", "105" }
            };
            ViewBag.Preguntas = API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args);
            return PartialView("_ReporteAcademicoDetallePartial");
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Académico")]
        public JsonResult CrearExcel(string Condicion, string CursoId, string NombreEmpleado, string DNIEmpleado)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "SedeId", "1" },
                { "EventoId", "1" },
                { "CursoId", CursoId },
                { "Condicion", Condicion},
                { "NombreEmpleado", NombreEmpleado },
                { "DNIEmpleado", DNIEmpleado }
            };
            byte[] ms = API.PostDownloadStream("ReporteAcademico/ReporteAcademicoExcel", arg);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Probando.xlsx");
            Response.BinaryWrite(ms);
            Response.End();

            return Json(Response);
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Académico")]
        public JsonResult DownloadFile(string documento)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "documento", documento },               
            };

            byte[] ms = API.PostDownloadStream("ReporteMultiple/DownloadFile", arg);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition","attachment; filename=FileName.pdf");
            Response.BinaryWrite(ms);
            Response.End();

            return Json(Response);
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Académico")]
        public JsonResult FichaValidacion()
        {
            Api API = new Api();
          
            byte[] ms = API.PostDownloadStream("ReporteMultiple/DownloadFileValidacion", null);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Probando.xlsx");
            Response.BinaryWrite(ms);
            Response.End();

            return Json(Response);
        }

        [GeneralSecurity(Rol = "Reportes-Reporte Académico")]
        public JsonResult ObtenerDatosEmpleado(string nroDocumento)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
                        {
                            { "nroDocumento", nroDocumento }
                        };
          var oEmpleado = API.Get<Empleado>("Empleado/DatosEmppleado", args);

            return new JsonResult { Data = oEmpleado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}