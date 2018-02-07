using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Cliente
{
    [Table("tblEmpleados")]
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        public int PersonaId { get; set; }
        public int EmpresaId { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }

    }
}
