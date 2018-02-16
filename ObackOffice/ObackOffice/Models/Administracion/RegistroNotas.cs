using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Administracion
{
    public class RegistroNotas
    {
        public int SalonProgramadoId { get; set; }
        public int EmpleadoId { get; set; }
        public int PersonaId { get; set; }
        public string NombreCompletoEmpleado { get; set; }
        public decimal Nota { get; set; }
        public string Taller { get; set; }
        public string Condicion { get; set; }
        public string Observacion { get; set; }
        public List<Asistencia> EmpleadoAsistencia { get; set; }
        public List<Taller> EmpleadoTaller { get; set; }
    }
}