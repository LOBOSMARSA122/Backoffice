using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Administracion
{
    public class Agenda
    {
        public int CursoProgramadoId { get; set; }
        public int EventoId { get; set; }
        public string Evento { get; set; }
        public int CursoId { get; set; }
        public string Curso { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}