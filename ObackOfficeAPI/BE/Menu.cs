using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public class Menu
    {
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public int PadreId { get; set; }
        public int EsEliminado { get; set; }
        public int UsuGraba { get; set; }
        public DateTime FechaGraba { get; set; }
        public int UsuActualiza { get; set; }
        public DateTime FechaActualiza { get; set; }
    }
}
