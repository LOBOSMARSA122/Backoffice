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
                int PreguntasGroupId = (int)Enumeradores.GrupoParametros.PreguntasTaller;

                var parametros = ctx.Parametros.ToList();

                var query = (from a in ctx.CursosProgramados
                             join b in ctx.Eventos on a.EventoId equals b.EventoId
                             join c in ctx.Cursos on a.CursoId equals c.CursoId
                             join e in ctx.SalonProgramados on a.CursoProgramadoId equals e.CursoProgramadoId
                             join f in ctx.SalonClases on e.SalonProgramadoId equals f.SalonProgramadoId
                             join g in ctx.EmpleadoCursos on f.SalonProgramadoId equals g.SalonProgramadoId
                             join h in ctx.EmpleadoAsistencias on g.EmpleadoCursoId equals h.EmpleadoCursoId
                             join i in ctx.Empleados on g.EmpleadoId equals i.EmpleadoId
                             join j in ctx.Personas on i.PersonaId equals j.PersonaId
                             join k in parametros on new { a = j.TipoDocumentoId, b = TipoDocumentoGroupId } equals new { a = k.ParametroId, b = k.GrupoId}
                             join l in parametros on new { a = b.SedeId, b = SedeGroupId } equals new { a = l.ParametroId, b = l.GrupoId }
                             join m in parametros on new { a = g.CondicionId, b = ConsidionGroupId } equals new { a = m.ParametroId, b = m.GrupoId }
                             join n in ctx.EventoSalones on b.EventoId equals n.EventoId
                             join o in ctx.EmpleadoTalleres on g.EmpleadoCursoId equals o.EmpleadoCursoId
                             join p in parametros on new { a = o.PreguntaId, b = PreguntasGroupId } equals new { a = p.ParametroId, b = p.GrupoId }
                             where 
                             (data.SedeId == -1 || data.SedeId == b.SedeId) &&
                             (data.EventoId == -1 || data.EventoId == b.EventoId) &&
                             (data.CursoId == -1 || data.CursoId == a.CursoId) &&
                             (string.IsNullOrWhiteSpace(data.NombreEmpleado) || (j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno).Contains(data.NombreEmpleado)) &&
                             (string.IsNullOrWhiteSpace(data.DNIEmpleado) || j.NroDocumento.Contains(data.DNIEmpleado)) &&
                             a.EsEliminado == NoEsEliminado
                             group new { a,b,c,e,f,g,h,i,j,k,l,m,n,o,p } by g.EmpleadoCursoId into grp
                             select new ReporteAcademicoList
                             {
                                 PersonaId = grp.FirstOrDefault().j.PersonaId,
                                 NroDocumento = grp.FirstOrDefault().j.NroDocumento,
                                 TipoDocumento = grp.FirstOrDefault().k.Valor1,
                                 Curso = grp.FirstOrDefault().c.NombreCurso,
                                 Evento = grp.FirstOrDefault().b.Nombre,
                                 Nombre = grp.FirstOrDefault().j.Nombres + " " + grp.FirstOrDefault().j.ApellidoPaterno + " " + grp.FirstOrDefault().j.ApellidoMaterno,
                                 Sede = grp.FirstOrDefault().l.Valor1,
                                 Condicion = grp.FirstOrDefault().m.Valor1,
                                 Nota = grp.FirstOrDefault().g.Nota,
                                 InicioCurso = grp.FirstOrDefault().a.FechaInicio,
                                 FinCurso = grp.FirstOrDefault().a.FechaFin,
                                 Observaciones = grp.FirstOrDefault().g.Observacion,
                                 ListaCursos = (from z in grp select 
                                                new ReporteAcademicoListClase
                                                {
                                                    Asistencia = z.h.Asistio == 1,
                                                    FechaClase = z.h.FechaClase,
                                                    Salon = z.n.Nombre
                                                }).ToList(),
                                 ListaTalleres = (from z in grp select
                                                  new ReporteAcademicoListTaller
                                                  {
                                                      Pregunta = z.p.Valor1,
                                                      Valor = z.o.Valor
                                                  }).ToList()
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
    }
}
