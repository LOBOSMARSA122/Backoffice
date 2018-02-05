using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Acceso
{
    public class Autorizacion
    {
        public int PerfilId { get; set; }
        public int RolId { get; set; }
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public int PadreId { get; set; }
        public string Icono { get; set; }
        public string Uri { get; set; }
        public List<SubMenu> SubMenus { get; set; }
    }
}
