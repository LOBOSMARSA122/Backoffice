using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Administracion
{
    public class EmpleadoAsistencia
    {
        public int EmpleadoAsistenciaId { get; set; }
        public int EmpleadoCursoId { get; set; }
        public DateTime FechaClase { get; set; }
        public int Asistio { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }

    public class Asistencia
    {
        public int EmpleadoAsistenciaId { get; set; }
        public int EmpleadoCursoId { get; set; }
        public DateTime FechaClase { get; set; }
        public int Asistio { get; set; }
    }
}