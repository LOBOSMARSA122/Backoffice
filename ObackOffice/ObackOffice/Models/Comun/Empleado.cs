using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Comun
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        public int PersonaId { get; set; }
        public int EmpresaId { get; set; }
        public String Cargo { get; set; }
        public String Area { get; set; }
    }
}