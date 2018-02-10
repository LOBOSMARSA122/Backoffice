using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Comun
{
    public class Bandejas
    {
        public int TotalRegistros { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BandejaUsuario : Bandejas
    {
        public string NombreUsuario { get; set; }
        public string NombrePersona { get; set; }

        public List<BandejaUsuarioLista> Lista { get; set; }
    }

    public class BandejaUsuarioLista
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
        public string Empresa { get; set; }
        public string TipoEmpresa { get; set; }
    }

    public class BandejaReporteAcademico : Bandejas
    {
        public int SedeId { get; set; }
        public int EventoId { get; set; }
        public int CursoId { get; set; }
        public string NombreEmpleado { get; set; }
        public string DNIEmpleado { get; set; }
        public List<ReporteAcademicoList> Lista { get; set; }
    }

    public class ReporteAcademicoList
    {
        public int CursoProgramadoId { get; set; }
        public int PersonaId { get; set; }
        public string Nombre { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Sede { get; set; }
        public string Evento { get; set; }
        public string Curso { get; set; }
        public decimal Nota { get; set; }
        public string Condicion { get; set; }
        public DateTime InicioCurso { get; set; }
        public DateTime FinCurso { get; set; }
        public string Observaciones { get; set; }
    }
    
    public class ReporteAcademicoListClase
    {
        public DateTime FechaClase { get; set; }
        public string Salon { get; set; }
        public string Asistencia { get; set; }
    }

    public class BandejaReporteMultiple : Bandejas
    {
        public int SedeId { get; set; }
        public int EventoId { get; set; }
        public int CursoId { get; set; }
        public string NombreEmpleado { get; set; }
        public string DNIEmpleado { get; set; }
        public int MaximasAsistencias { get; set; }
        public List<ReporteMultipleList> Lista { get; set; }
    }

    public class ReporteMultipleList
    {
        public int EmpleadoCursoId { get; set; }
        public int PersonaId { get; set; }
        public string Nombre { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Sede { get; set; }
        public string Evento { get; set; }
        public string Curso { get; set; }
        public decimal Nota { get; set; }
        public string NotaTaller { get; set; }
        public string Condicion { get; set; }
        public List<string> Asistencia { get; set; }
    }
}
