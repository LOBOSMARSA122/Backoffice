using ObackOffice.Controllers.Seguridad;
using ObackOffice.Models;
using ObackOffice.Models.Administracion;
using ObackOffice.Models.Cliente;
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
        public ActionResult Agenda()
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Sedes).ToString() },
                { "accion",Constantes.Select },
            };
            ViewBag.EVENTOS = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.Select);

            return View();
        }

        public JsonResult GetCursosProgramados(string cursoId)
        {
            //if (cursoId != "-1")
            //{
                Api API = new Api();
                string url = "CursoProgramado/CursosProgramados";
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add("cursoId", cursoId);
                List<Agenda> result = API.Get<List<Agenda>>(url, args);
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //}
            //return new JsonResult { Data = new List<Agenda>(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetEvento(string sedeId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "sedeId",sedeId }
            };
            List<Dropdownlist> Eventos = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Eventos/ddlEventos", args), Constantes.Select);            
            return new JsonResult { Data = Eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetCurso(string eventoId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "eventoId",eventoId }
            };
            List<Dropdownlist> Eventos = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("CursoProgramado/ddlCursoProgramdos", args), Constantes.Select);
            return new JsonResult { Data = Eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSalon(string cursoProgramadoId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "cursoProgramadoId",cursoProgramadoId }
            };
            List<Dropdownlist> Eventos = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("CursoProgramado/ddlSalonProgramado", args), Constantes.Select);
            return new JsonResult { Data = Eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult EmpleadosInscritos(string salonProgramadoId)
        {
            if (salonProgramadoId != "-1")
            {
                Api API = new Api();
                ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
                Dictionary<string, string> args = new Dictionary<string, string>
                {
                    { "salonProgramadoId", salonProgramadoId}
                };
                ViewBag.EMPLEADOSINSCRITOS = API.Get<List<EmpleadoInscrito>>("CursoProgramado/GetEmpleadosCurso", args);
                              
            }
            else
            {
                ViewBag.EMPLEADOSINSCRITOS = null;
            }

            return PartialView("_ListaEmpleadosIsncritosPartial");
        }

        public ActionResult InformacionCurso(string salonProgramadoId)
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Dictionary<string, string> args = new Dictionary<string, string>
                {
                    { "salonProgramadoId", salonProgramadoId}
                };
            ViewBag.INFORMACIONCURSO = API.Get<InformacionSalonProgramado>("CursoProgramado/GetInformacionCurso", args);
            return PartialView("_InformacionCursoPartial");
        }

        public JsonResult GetEmpleado(string valor, string empresaId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "valor",valor },
                {"empresaId", empresaId }
            };
            List<string> Eventos = API.Get<List<string>>("Empleado/GetEmpleados", args);
            return new JsonResult { Data = Eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InscribirEmpleado(string empleado, string salonProgramadoId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                {"empleado",empleado },
                {"salonProgramadoId", salonProgramadoId },
                {"userId",  ((ClientSession)Session["AutBackoffice"]).UsuarioId.ToString() }
            };
            bool result = API.Get<bool>("CursoProgramado/InsertarEmpleadoCurso", args);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult EliminarInscripcion(string empleadoCursoId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {           
                {"empleadoCursoId", empleadoCursoId },
                {"userId",  ((ClientSession)Session["AutBackoffice"]).UsuarioId.ToString() }
            };
            bool result = API.Get<bool>("CursoProgramado/EliminarEmpleadoCurso", args);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GrabarEmpleado(dataEmpleado data)
        {
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "PersonaId",data.PersonaId.ToString() },
                { "EmpresaId", ViewBag.USUARIO.EmpresaId.ToString()},
                { "Cargo",data.Cargo.ToString()},
                { "UsuGraba", ViewBag.USUARIO.UsuarioId.ToString()},

                { "Nombres", data.Nombres.ToString()},
                { "ApePaterno", data.ApePaterno.ToString()},
                { "ApeMaterno", data.ApeMaterno.ToString()},
                { "TipoDocumentoId", data.TipoDocumentoId.ToString()},
                { "NroDocumento", data.NroDocumento.ToString()},
            };
            var result= API.Post<bool>("Empleado/GrabarEmpleado", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Empleado-Historial de Notas")]
        public ActionResult HistorialDeNotas()
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "usuarioId", ViewBag.USUARIO.UsuarioId.ToString() }
            };
            List<ReporteMultipleList> data = API.Get<List<ReporteMultipleList>>("Empleado/ObtenerHistorialEmpleado",arg);

            return View(data);
        }
    }
}