using BE.Administracion;
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

        [HttpGet]
        public IHttpActionResult GetEventos(int empresaId, string nombreEvento)
        {
            List<BandejaEventos> result = er.GetEventos(empresaId, nombreEvento);
            return Ok(result);
        }
    }
}
