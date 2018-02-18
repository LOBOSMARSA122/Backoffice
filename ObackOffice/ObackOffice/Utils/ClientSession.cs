using ObackOffice.Models.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Utils
{
    public class ClientSession
    {
        public int UsuarioId { get; set; }
        public int PersonaId { get; set; }
        public int EmpresaId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime? FechaCaduca { get; set; }
        public int RolId { get; set; }
        public string Rol { get; set; }
        public byte[] foto { get; set; }
        public List<Autorizacion> Autorizacion { get; set; }

    }
}