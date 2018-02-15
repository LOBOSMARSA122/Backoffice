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
    public class RegistroNotasController : Controller
    {
        // GET: RegistroNotas
        public ActionResult BandejaRegistroNotas()
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
          
            ViewBag.CAPACITADOR = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Capacitador/ddlCapacitador", null), Constantes.All);
            ViewBag.CURSO = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Curso/ddlCurso", null), Constantes.All);
            
            return View();
        }

        public ActionResult Filtrar(BandejaRegistroNotas data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "fechaInicio",data.fechaInicio.ToString() },
                { "fechaFin", data.fechaFin.ToString()},
                { "capacitadorId",data.capacitadorId.ToString()},
                { "cursoId", data.cursoId.ToString()},
                { "activoId", data.activoId.ToString()},
                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };
            ViewBag.BandejaRegistro = API.Post<BandejaRegistroNotas>("RegistroNotas/GetRegistroNotas", arg);
            return PartialView("_BandejaRegistroNotasPartial");
        }

        public ActionResult RegistarNotas(int salonProgramadoId)
        {
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            ViewBag.SALONPROGRAMADO = salonProgramadoId;
            return View();
        }

    }
}