using BE.Administracion;
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

    }
}
