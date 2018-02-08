using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models.Cliente
{
    public class EmpleadoCurso
    {
        public int EmpleadoCursoId { get; set; }
        public int EmpleadoId { get; set; }
        public int SalonProgramadoId { get; set; }
        public decimal Nota { get; set; }
        public int CondicionId { get; set; }
        public string Observacion { get; set; }

        public int EsEliminado { get; set; }
        public int? UsuGraba { get; set; }
        public DateTime? FechaGraba { get; set; }
        public int? UsuActualiza { get; set; }
        public DateTime? FechaActualiza { get; set; }
    }

    public class EmpleadoInscrito
    {
        public int PersonaId { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoDocumento { get; set; }        
        public string NroDocumento { get; set; }
    }

    public class InformacionSalonProgramado
    {
        public string NombreCapacitador { get; set; }
        public string EspecialidadCapacitador { get; set; }
        public byte[] FotoCapacitador { get; set; }
        public string ExperienciaCapacitador { get; set; }
        public int CuposDisponibles { get; set; }
        public int CuposTotales { get; set; }
        public DateTime FechaInicioCurso { get; set; }
        public DateTime FechaFinCurso { get; set; }
        public List<ClasesProgramada> ClasesProgramadas { get; set; }
    }

    public class ClasesProgramada
    {
        public DateTime FechaInicioClase { get; set; }
        public DateTime FechafinClase { get; set; }
    }
}