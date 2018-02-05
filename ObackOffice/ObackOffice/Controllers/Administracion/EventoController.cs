using ObackOffice.Models;
using ObackOffice.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Administracion
{
    public class EventoController : Controller
    {
  
        public ActionResult BandejaEvento()
        {
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Api API = new Api();
            string url = "Eventos/GetEventos";
            Dictionary<string, string> Filtro = new Dictionary<string, string>();
            Filtro.Add("empresaId", "-1");
            Filtro.Add("NombreEvento", "");
            ViewBag.EVENTOS = API.Get<List<Models.Administracion.BandejaEventos>>(url, Filtro);
            return View(ViewBag.EVENTOS);
        }

        public ActionResult Evento()
        {
            return View();
        }

        //public JsonResult GetEventos()
        //{

        //}
    }
}