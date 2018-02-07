using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Administracion
{
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

    public class ddlEvento
    {
        public int EventoId { get; set; }
        public string Nombre { get; set; }
    }
}