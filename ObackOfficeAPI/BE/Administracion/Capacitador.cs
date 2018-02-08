using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Administracion
{
    [Table("tblCapacitadores")]
    public class Capacitador
    {
        public int CapacitadorId { get; set; }
        public int PersonaId { get; set; }
        public string Curriculo { get; set; }
        public string Especialidad { get; set; }
        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }
}
