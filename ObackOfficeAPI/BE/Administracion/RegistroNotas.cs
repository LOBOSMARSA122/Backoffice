using BE.RegistroNotas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Administracion
{
    public class RegistroNotas
    {
        public string Capacitador { get; set; }
        public DateTime? FechaInicioCurso { get; set; }
        public DateTime? FechaFinCurso { get; set; }
        public int NroCupos { get; set; }
        public string Curso { get; set; }

        public int EmpleadoCursoId { get; set; }
        public int SalonProgramadoId { get; set; }
        public int EmpleadoId { get; set; }
        public int PersonaId { get; set; }
        public string NombreCompletoEmpleado { get; set; }
        public decimal Nota { get; set; }
        public string Taller { get; set; }
        public int CondicionId { get; set; }
        public string Observacion { get; set; }
        public List<Asistencia> EmpleadoAsistencia { get; set; }
        public List<Taller> EmpleadoTaller { get; set; }
    }

    
}
