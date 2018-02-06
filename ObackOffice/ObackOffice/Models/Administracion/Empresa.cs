using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Administracion
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public string RazonSocial { get; set; }
        public string Ruc { get; set; }
        public int TipoEmpresaId { get; set; }
        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }
}