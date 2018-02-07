using BE.Administracion;
using BE.Comun;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Administracion
{
    public class EventosController : ApiController
    {
        private EventoReporsitory er = new EventoReporsitory();
        private CursoProgramadoRepository cpr = new CursoProgramadoRepository();

        [HttpGet]
        public IHttpActionResult GetEventos(int empresaId, string nombreEvento)
        {
            List<BandejaEventos> result = er.GetEventos(nombreEvento);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAgenda(int eventoId)
        {
            List<Agenda> result = cpr.GetAgenda(eventoId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ddlEventos(int sedeId)
        {
            List<Dropdownlist> result = er.ddlEventos(sedeId);
            return Json(result);
        }

        [HttpGet]
        public IHttpActionResult ddlCursoProgramdos(int eventoId)
        {
            List<Dropdownlist> result = cpr.ddlCursoProgramdos(eventoId);
            return Ok(result);
        }
    }
}
