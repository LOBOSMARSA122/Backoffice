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
                             join d in ctx.Parametros on new { a = a.CursoId, b = 108 } equals new { a = d.ParametroId, b = d.GrupoId }
                             where a.CursoId == cursoId && a.EsEliminado == 0
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
                             where a.SalonProgramadoId == salonProgramadoId
                             select new EmpleadoInscrito
                             {
                                 PersonaId = c.PersonaId,
                                 NombreCompleto = c.Nombres + " " + c.ApellidoPaterno + " " + c.ApellidoMaterno,
                                 TipoDocumento = f.Valor1,
                                 NroDocumento = c.NroDocumento
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

    }
}
