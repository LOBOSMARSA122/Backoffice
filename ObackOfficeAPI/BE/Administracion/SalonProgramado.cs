using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Administracion
{
    [Table("tblSalonProgramado")]
    public class SalonProgramado
    {
        public int SalonProgramadoId { get; set; }
        public int CursoProgramadoId { get; set; }
        public int? CapacitadorId { get; set; }       
        public int EventoSalonId { get; set; }
        public int NroCupos { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }
}
