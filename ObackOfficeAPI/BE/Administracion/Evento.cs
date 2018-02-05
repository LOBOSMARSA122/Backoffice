using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Administracion
{
    [Table("tblEventos")]
    public class Evento
    {
        public int EventoId { get; set; }
        public int EmpresaId { get; set; }
        public string Nombre { get; set; }
        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }

    public class BandejaEventos
    {
        public int EventoId { get; set; }
        public int EmpresaId { get; set; }
        public string Empresa { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }       
    }
}
