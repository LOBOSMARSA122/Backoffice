using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models
{
    public class Bandejas
    {
        public int TotalRegistros { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BandejaUsuario : Bandejas
    {
        public string NombreUsuario { get; set; }
        public string NombrePersona { get; set; }
        
        public List<BandejaUsuarioLista> Lista { get; set; }
    }

    public class BandejaUsuarioLista
    {
       public int UsuarioId { get; set; }
       public string NombreUsuario { get; set; }
       public string NombreCompleto { get; set; }
       public string Rol { get; set; }
       public string Empresa { get; set; }
       public string TipoEmpresa { get; set; }
    }
}