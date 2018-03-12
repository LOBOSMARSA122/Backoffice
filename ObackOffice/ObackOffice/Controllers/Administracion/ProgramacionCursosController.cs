using ObackOffice.Controllers.Seguridad;
using ObackOffice.Models;
using ObackOffice.Models.Comun;
using ObackOffice.Models.Administracion;
using Newtonsoft.Json;
using ObackOffice.Utils;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace ObackOffice.Controllers.Administracion
{
    public class ProgramacionCursosController : Controller
    {
        [GeneralSecurity(Rol = "Administración-Programación de Cursos")]
        public ActionResult Index()
        {
            Api API = new Api();

            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Sedes).ToString() }
            };
            ViewBag.SEDES = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.All);
            ViewBag.CURSOS = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Curso/ddlCurso"), Constantes.All);
            ViewBag.CAPACITADOR = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Capacitador/ddlCapacitador"), Constantes.Select);

            return View();
        }

        [GeneralSecurity(Rol = "Administración-Programación de Cursos")]
        public JsonResult FiltrarCalendario(string SedeId, string CursoId, string year, string month)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "SedeId",SedeId },
                { "EventoId", "1" },
                { "CursoId",CursoId },
                { "year",year },
                { "month",month }
            };

            List<Agenda> Result = API.Get<List<Agenda>>("ProgramacionCursos/FiltrarCalendario", args);

            return Json(Result);
        }

        [GeneralSecurity(Rol = "Administración-Programación de Cursos")]
        public JsonResult SaveData(string data)
        {
            Api API = new Api();

            ProgramacionCursos prog = JsonConvert.DeserializeObject<ProgramacionCursos>(data);
            prog.EventoId = 1;
            prog.UsuarioActualizaID = ViewBag.USUARIO.UsuarioId;

            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", JsonConvert.SerializeObject(prog) }
            };
            bool saved = API.Post<bool>("ProgramacionCursos/ProgramacionCursoDataProcess", args);

            if (saved)
                return Json(saved);
            else
                return Json(null);
        }

        [GeneralSecurity(Rol = "Administración-Programación de Cursos")]
        public JsonResult GetCalendarEvent(string id)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "id",id }
            };
            ProgramacionCursos data = API.Get<ProgramacionCursos>("ProgramacionCursos/GetCalendarEvent", args);
            return Json(data);
        }
    }
}