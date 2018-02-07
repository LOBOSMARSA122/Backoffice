using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Administracion
{
    [Table("tblCursoProgramado")]
    public class CursoProgramado
    {
        public int CursoProgramadoId { get; set; }
        public int EventoId { get; set; }
        public int CursoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }

    public class Agenda
    {
        public int CursoProgramadoId { get; set; }
        public int EventoId { get; set; }
        public string Evento { get; set; }
        public int CursoId { get; set; }
        public string Curso { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class ddlCursoProgramdo
    {
        public int CursoId { get; set; }
        public string Nombre { get; set; }
    }
}
