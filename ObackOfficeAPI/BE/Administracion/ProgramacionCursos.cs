using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Administracion
{
    public class ProgramacionCursos
    {
        public int RecordType { get; set; }
        public int RecordStatus { get; set; }
        public int CursoProgramadoId { get; set; }
        public int SedeId { get; set; }
        public int EventoId { get; set; }
        public int CursoId { get; set; }
        public int UsuarioActualizaID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<ProgramacionCursosSalones> Salones { get; set; }
    }

    public class ProgramacionCursosSalones
    {
        public int RecordType { get; set; }
        public int RecordStatus { get; set; }
        public int SalonId { get; set; }
        public int CapacitadorId { get; set; }
        public int Cupos { get; set; }
        public int EsEliminado { get; set; }
        public List<ProgramacionCursosClases> Clases { get; set; }
    }

    public class ProgramacionCursosClases
    {
        public int RecordType { get; set; }
        public int RecordStatus { get; set; }
        public int ClaseId { get; set; }
        public int EsEliminado { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
    }
}
