using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Acceso
{
    [Table("tblPerfiles")]
    public class Perfil
    {
        public int PerfilId { get; set; }
        public int RolId { get; set; }
        public int MenuId { get; set; }
        public int EsEliminado { get; set; }
        public int UsuGraba { get; set; }
        public DateTime FechaGraba { get; set; }
        public int UsuActualiza { get; set; }
        public DateTime FechaActualiza { get; set; }
    }
}
