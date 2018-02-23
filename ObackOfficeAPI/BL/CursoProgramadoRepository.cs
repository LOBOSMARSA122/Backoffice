using BE.Administracion;
using BE.Cliente;
using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CursoProgramadoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Agenda> CursosProgramados(int cursoId)
        {
            try
            {
                var query = (from a in ctx.CursosProgramados
                             join b in ctx.Eventos on a.EventoId equals b.EventoId
                             join c in ctx.Cursos on a.CursoId equals c.CursoId
                             join d in ctx.Parametros on new { a = c.ColorId, b = 108 } equals new { a = d.ParametroId, b = d.GrupoId }
                             where a.EsEliminado == 0
                                    //&& a.CursoId == cursoId  
                             select new Agenda
                             {
                                 CursoProgramadoId = a.CursoProgramadoId,
                                 EventoId = a.EventoId,
                                 Evento = b.Nombre,
                                 CursoId = a.CursoId,
                                 Curso = c.NombreCurso,
                                 Color = d.Valor2,
                                 FechaInicio = a.FechaInicio,
                                 FechaFin = a.FechaFin
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<EmpleadoInscrito> GetEmpleadosCurso(int salonProgramadoId)
        {
            try
            {
                var query = (from a in ctx.EmpleadoCursos
                             join b in ctx.Empleados on a.EmpleadoId equals b.EmpleadoId
                             join c in ctx.Personas on b.PersonaId equals c.PersonaId
                             join d in ctx.SalonProgramados on a.SalonProgramadoId equals d.SalonProgramadoId
                             join e in ctx.CursosProgramados on d.CursoProgramadoId equals e.CursoProgramadoId
                             join f in ctx.Parametros on new { a = c.TipoDocumentoId, b = 101 } equals new { a = f.ParametroId, b = f.GrupoId }
                             where a.SalonProgramadoId == salonProgramadoId && a.EsEliminado==0
                             select new EmpleadoInscrito
                             {
                                 EmpleadoCursoId = a.EmpleadoCursoId,
                                 PersonaId = c.PersonaId,
                                 NombreCompleto = c.Nombres + " " + c.ApellidoPaterno + " " + c.ApellidoMaterno,
                                 TipoDocumento = f.Valor1,
                                 NroDocumento = c.NroDocumento,
                                 NroCupos = d.NroCupos
                             }
                             ).ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Dropdownlist> ddlCursoProgramdos(int eventoId)
        {
            try
            {
                var query = (from a in ctx.CursosProgramados
                             join b in ctx.Cursos on a.CursoId equals b.CursoId
                             where a.EventoId == eventoId && a.EsEliminado == 0
                             select new Dropdownlist
                             {
                                 Id = a.CursoId,
                                 Value = b.NombreCurso
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Agenda> GetAgenda(int eventoId)
        {
            try
            {
                var query = (from a in ctx.CursosProgramados
                             join b in ctx.Eventos on a.EventoId equals b.EventoId
                             join c in ctx.Parametros on new { a = a.CursoId, b = 103 } equals new { a = c.ParametroId, b = c.GrupoId }
                             where a.EventoId == eventoId && a.EsEliminado == 0
                             select new Agenda
                             {
                                 CursoProgramadoId = a.CursoProgramadoId,
                                 EventoId = a.EventoId,
                                 Evento = b.Nombre,
                                 CursoId = a.CursoId,
                                 Curso = c.Valor1,
                                 FechaInicio = a.FechaInicio,
                                 FechaFin = a.FechaFin
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<Dropdownlist> ddlSalonProgramado(int cursoProgramadoId)
        {
            try
            {
                var query = (from a in ctx.SalonProgramados
                             join b in ctx.EventoSalones on a.EventoSalonId equals b.EventoSalonId
                             where a.CursoProgramadoId == cursoProgramadoId && a.EsEliminado == 0
                             select new Dropdownlist
                             {
                                 Id = a.SalonProgramadoId,
                                 Value = b.Nombre
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public InformacionSalonProgramado GetInformacionSalonProgramado(int salonProgramadoId)
        {
            try
            {
                var query = (from a in ctx.SalonClases
                             join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                             join c in ctx.Capacitadores on b.CapacitadorId equals c.CapacitadorId
                             join d in ctx.Personas on c.PersonaId equals d.PersonaId
                             join e in ctx.CursosProgramados on b.CursoProgramadoId equals e.CursoProgramadoId
                             where b.SalonProgramadoId == salonProgramadoId
                             group new { a, b, c, d, e } by b.SalonProgramadoId into grp
                             select new InformacionSalonProgramado
                             {
                                 NombreCapacitador = grp.FirstOrDefault().d.Nombres + " " + grp.FirstOrDefault().d.ApellidoPaterno,
                                 EspecialidadCapacitador = grp.FirstOrDefault().c.Especialidad,
                                 FotoCapacitador = grp.FirstOrDefault().d.Foto,
                                 ExperienciaCapacitador = grp.FirstOrDefault().c.Curriculo,
                                 CuposTotales = grp.FirstOrDefault().b.NroCupos,
                                 CuposDisponibles = grp.FirstOrDefault().b.NroCupos - (from z in ctx.EmpleadoCursos where z.SalonProgramadoId == salonProgramadoId && z.EsEliminado == 0 select z).Count(),
                                 FechaInicioCurso = grp.FirstOrDefault().e.FechaInicio,
                                 FechaFinCurso = grp.FirstOrDefault().e.FechaFin,
                                 ClasesProgramadas = (from z in grp select
                                                      new ClasesProgramada
                                                      {
                                                          FechaInicioClase = z.a.FechaInicio,
                                                          FechafinClase = z.a.FechaFin
                                                      }).ToList(),

                             }).ToList();
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
