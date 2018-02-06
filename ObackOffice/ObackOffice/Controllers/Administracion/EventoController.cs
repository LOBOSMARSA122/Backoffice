using ObackOffice.Models;
using ObackOffice.Models.Administracion;
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

        public ActionResult Evento(int? EventoId)
        {
            if (EventoId.HasValue)
            {
                ViewBag.EventoId = EventoId;
            }
            else
            {
                ViewBag.EventoId = 0;
            }
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            return View();
        }

        public JsonResult GetAgenda(int? eventoId)
        {
            var x = ViewBag.EventoId;
            Api API = new Api();
            string url = "Eventos/GetAgenda";
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("eventoId", eventoId.ToString());
            List<Agenda> Agenda = API.Get<List<Agenda>>(url, args);
            return new JsonResult { Data = Agenda, JsonRequestBehavior =JsonRequestBehavior.AllowGet };
        }
    }
}