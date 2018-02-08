using BE.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Cliente
{
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
}
