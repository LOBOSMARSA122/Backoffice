using BL;
using BE.Administracion;
using System.Collections.Generic;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Administracion
{
    public class ProgramacionCursosController : ApiController
    {
        private ProgramacionCursosRepository pcr = new ProgramacionCursosRepository();

        [HttpGet]
        public IHttpActionResult FiltrarCalendario(int SedeId, int EventoId, int CursoId, int year, int month)
        {
            if (SedeId == 0 || EventoId == 0 || CursoId == 0 || year == 0 || month == 0)
                return BadRequest("Alguno de los parámetros es incorrecto.");

            List<Agenda> result = pcr.GetAllAgenda(SedeId, EventoId, CursoId, year, month);

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetCalendarEvent(int id)
        {
            if (id == 0)
                return BadRequest("Parámetro Incorrecto");

            ProgramacionCursos response = pcr.GetCalendarEvent(id);

            return Ok(response);
        }
    }
}
