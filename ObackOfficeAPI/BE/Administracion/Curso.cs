using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Administracion
{
    [Table("tblCursos")]
    public class Curso
    {
        public int CursoId { get; set; }
        public string NombreCurso { get; set; }
        public string CodigoCurso { get; set; }
        public int NroHoras { get; set; }
        public string Descripcion { get; set; }
        public int ColorId { get; set; }
        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }
}
