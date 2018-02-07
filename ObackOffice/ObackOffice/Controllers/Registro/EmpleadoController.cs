using ObackOffice.Models;
using ObackOffice.Models.Administracion;
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

        public JsonResult GetEvento(string sedeId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "sedeId",sedeId },
                { "accion",Constantes.Select },
            };
            List<Dropdownlist> Eventos = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Eventos/ddlEventos", args), Constantes.Select);            
            return new JsonResult { Data = Eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetCurso(string eventoId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "eventoId",eventoId },
                { "accion",Constantes.Select },
            };
            List<Dropdownlist> Eventos = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("CursoProgramado/ddlCursoProgramdos", args), Constantes.Select);
            return new JsonResult { Data = Eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}