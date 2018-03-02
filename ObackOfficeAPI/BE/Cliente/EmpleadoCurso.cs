using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Cliente
{
    [Table("tblEmpleadoCursos")]
    public class EmpleadoCurso { 
        public int EmpleadoCursoId { get; set; }
        public int EmpleadoId { get; set; }
        public int SalonProgramadoId { get; set; }
        public int? EmpresaId { get; set; }
        public decimal Nota { get; set; }
        public int CondicionId { get; set; }
        public string Observacion { get; set; }
        public string NotaTaller { get; set; }
        public decimal NotaFinal { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }

    public class EmpleadoInscrito
    {
        public int EmpleadoCursoId { get; set; }
        public int PersonaId { get; set; }
        public string Empresa { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public int NroCupos { get; set; }
    }
}
