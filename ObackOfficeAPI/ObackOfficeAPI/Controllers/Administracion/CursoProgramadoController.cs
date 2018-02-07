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
    public class CursoProgramadoController : ApiController
    {
        private CursoProgramadoRepository cpr = new CursoProgramadoRepository();

        [HttpGet]
        public IHttpActionResult CursosProgramados(int cursoId)
        {
            List<Agenda> result = cpr.CursosProgramados(cursoId);
            return Ok(result);
        }


        [HttpGet]
        public IHttpActionResult ddlCursoProgramdos(int eventoId)
        {
            List<Dropdownlist> result = cpr.ddlCursoProgramdos(eventoId);
            return Ok(result);
        }


    }
}
