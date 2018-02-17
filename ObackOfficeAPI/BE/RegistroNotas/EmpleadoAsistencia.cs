using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.RegistroNotas
{
    [Table("tblEmpleadoAsistencia")]
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
        public int RecordStatus { get; set; }        
    }
}
