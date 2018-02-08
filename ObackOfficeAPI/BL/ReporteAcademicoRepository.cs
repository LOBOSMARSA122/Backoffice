using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ReporteAcademicoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public BandejaReporteAcademico BandejaReporteAcademico(BandejaReporteAcademico data)
        {
            try
            {
                int NoEsEliminado = (int)Enumeradores.EsEliminado.No;
                int TipoDocumentoGroupId = (int)Enumeradores.GrupoParametros.TipoDocumentos;
                int SedeGroupId = (int)Enumeradores.GrupoParametros.Sedes;
                int ConsidionGroupId = (int)Enumeradores.GrupoParametros.Condición;
                string NombreEmpleado = string.IsNullOrWhiteSpace(data.NombreEmpleado) ? null : data.NombreEmpleado;
                string DNIEmpleado = string.IsNullOrWhiteSpace(data.DNIEmpleado) ? null : data.DNIEmpleado;

                var query = (from a in ctx.CursosProgramados
                             join b in ctx.Eventos on a.EventoId equals b.EventoId
                             join c in ctx.Cursos on a.CursoId equals c.CursoId
                             join e in ctx.SalonProgramados on a.CursoProgramadoId equals e.CursoProgramadoId
                             join g in ctx.EmpleadoCursos on e.SalonProgramadoId equals g.SalonProgramadoId
                             join f in ctx.SalonClases on e.SalonProgramadoId equals f.SalonProgramadoId
                             
                             join i in ctx.Empleados on g.EmpleadoId equals i.EmpleadoId
                             join j in ctx.Personas on i.PersonaId equals j.PersonaId
                             join k in ctx.Parametros on new { a = j.TipoDocumentoId, b = TipoDocumentoGroupId } equals new { a = k.ParametroId, b = k.GrupoId }
                             join l in ctx.Parametros on new { a = b.SedeId, b = SedeGroupId } equals new { a = l.ParametroId, b = l.GrupoId }
                             join m in ctx.Parametros on new { a = g.CondicionId, b = ConsidionGroupId } equals new { a = m.ParametroId, b = m.GrupoId }
                             where 
                             (data.SedeId == -1 || data.SedeId == b.SedeId) &&
                             (data.EventoId == -1 || data.EventoId == b.EventoId) &&
                             (data.CursoId == -1 || data.CursoId == a.CursoId) &&
                             (NombreEmpleado == null || (j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno).Contains(NombreEmpleado)) &&
                             (DNIEmpleado == null || j.NroDocumento.Contains(DNIEmpleado)) &&
                             a.EsEliminado == NoEsEliminado &&
                             f.SalonProgramadoId == e.SalonProgramadoId
                             select new ReporteAcademicoList
                             {
                                 CursoProgramadoId = a.CursoProgramadoId,
                                 PersonaId = j.PersonaId,
                                 NroDocumento = j.NroDocumento,
                                 TipoDocumento = k.Valor1,
                                 Curso = c.NombreCurso,
                                 Evento = b.Nombre,
                                 Nombre = j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno,
                                 Sede = l.Valor1,
                                 Condicion = m.Valor1,
                                 Nota = g.Nota,
                                 InicioCurso = a.FechaInicio,
                                 FinCurso = a.FechaFin,
                                 Observaciones = g.Observacion
                             }).ToList();

                BandejaReporteAcademico return_data = new BandejaReporteAcademico()
                {
                    Lista = query
                };
                return return_data;
            }
            catch(Exception e)
            {
                return null;
            }   
        }

        public List<ReporteAcademicoListClase> DetalleEmpleado(int PersonaId, int cursoProgramadoId)
        {
            var return_data = (from a in ctx.SalonProgramados
                        join b in ctx.EventoSalones on a.EventoSalonId equals b.EventoSalonId
                        join c in ctx.EmpleadoCursos on a.SalonProgramadoId equals c.SalonProgramadoId
                        join d in ctx.EmpleadoAsistencias on c.EmpleadoCursoId equals d.EmpleadoCursoId
                        join e in ctx.Empleados on c.EmpleadoId equals e.EmpleadoId
                        where e.PersonaId == PersonaId &&
                        a.CursoProgramadoId == cursoProgramadoId
                        select new ReporteAcademicoListClase
                        {
                            Asistencia = d.Asistio == 1,
                            Salon = b.Nombre,
                            FechaClase = d.FechaClase
                        }).ToList();

            return return_data;
        }
    }
}
