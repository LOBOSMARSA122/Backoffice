using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Administracion
{
    public class EmpleadoTaller
    {
        public int EmpleadoTallerId { get; set; }
        public int EmpleadoCursoId { get; set; }
        public int PreguntaId { get; set; }
        public string Valor { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }

    public class Taller
    {
        public int EmpleadoTallerId { get; set; }
        public int EmpleadoCursoId { get; set; }
        public string Pregunta { get; set; }
        public int PreguntaId { get; set; }
        public string Valor { get; set; }
    }
}