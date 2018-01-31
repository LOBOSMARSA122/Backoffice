using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Acceso
{
    public class UsuarioLogin
    {
        public int UsuarioId { get; set; }
        public int PersonaId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Contrasenia { get; set; }
        public DateTime FechaCaduca { get; set; }
        public int RolId { get; set; }
        public string Rol { get; set; }
        public List<Autorizacion> Autorizacion { get; set; }
    }
}