using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    [Table("tblUsuarios")]
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public int PersonaId { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string PreguntaSecreta { get; set; }
        public string RespuestaSecreta { get; set; }
        public DateTime FechaCaduca { get; set; }
        public int RolId { get; set; }

        public int EsEliminado { get; set; }
        public int UsuGraba { get; set; }
        public DateTime FechaGraba { get; set; }
        public int UsuActualiza { get; set; }
        public DateTime FechaActualiza { get; set; }
    }
}
