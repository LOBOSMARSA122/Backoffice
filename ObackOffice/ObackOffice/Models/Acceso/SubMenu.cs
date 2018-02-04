using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Acceso
{
    public class SubMenu
    {
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public int PadreId { get; set; }
        public string Icono { get; set; }
    }
}