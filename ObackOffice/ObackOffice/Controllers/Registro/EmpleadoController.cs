﻿using ObackOffice.Models;
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

        public JsonResult GetCursosProgramados(int cursoId)
        {
            Api API = new Api();
            string url = "CursoProgramado/CursosProgramados";
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("cursoId", cursoId.ToString());
            List<Agenda> result = API.Get<List<Agenda>>(url, args);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "salonProgramadoId", salonProgramadoId}
            };
            ViewBag.EMPLEADOSINSCRITOS = API.Get<List<EmpleadoInscrito>>("CursoProgramado/GetEmpleadosCurso", args);

            return PartialView("_ListaEmpleadosIsncritosPartial");
        }
    }
}