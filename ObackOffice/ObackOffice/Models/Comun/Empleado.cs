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

    public class dataEmpleado
    {
        public int EmpleadoId { get; set; }
        public int PersonaId { get; set; }
        public int EmpresaId { get; set; }
        public String Cargo { get; set; }
        public String Area { get; set; }

        public String Nombres { get; set; }
        public String ApePaterno { get; set; }
        public String ApeMaterno { get; set; }
        public int TipoDocumentoId { get; set; }
        public String NroDocumento { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }

    }
}