using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Comun
{
    [Table("tblParametros")]
    public class Parametro
    {
        [Key]
        [Column(Order = 1)]
        public int GrupoId { get; set; }
        [Key]
        [Column(Order = 2)]
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
}
