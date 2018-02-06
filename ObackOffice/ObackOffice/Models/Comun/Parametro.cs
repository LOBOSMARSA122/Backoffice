using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Comun
{
    public class Parametro
    {      
        public int GrupoId { get; set; }      
        public int ParametroId { get; set; }
        public string Valor1 { get; set; }
        public string Valor2 { get; set; }
        public string Campo { get; set; }
        public int PadreParametroId { get; set; }
        public int Orden { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }

    public class Dropdownlist
    {
        public int ParametroId { get; set; }
        public string Valor1 { get; set; }
    }
}