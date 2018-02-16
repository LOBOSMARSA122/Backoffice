using System;
using BE.Administracion;
using BE.Comun;
using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BL
{
    public class ProgramacionCursosRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Agenda> GetAllAgenda(int SedeId, int EventoId, int CursoId, int year, int month)
        {
            try
            {
                DateTime FeIni = new DateTime(year, month, 1);
                DateTime FeFin = new DateTime(year, month, 1).AddMonths(1);

                var return_data = (from a in ctx.CursosProgramados
                                   join b in ctx.Eventos on a.EventoId equals b.EventoId
                                   join c in ctx.Cursos on a.CursoId equals c.CursoId
                                   join d in ctx.Parametros on new { a = a.CursoId, b = 108 } equals new { a = d.ParametroId, b = d.GrupoId }
                                   where
                                   (SedeId == -1 || b.SedeId == SedeId) &&
                                   (EventoId == -1 || b.EventoId == EventoId) &&
                                   (CursoId == -1 || c.CursoId == CursoId) &&
                                   ((a.FechaInicio > FeIni && a.FechaFin < FeFin) ||
                                   (a.FechaInicio < FeIni && a.FechaFin > FeIni) ||
                                   (a.FechaInicio < FeFin && a.FechaFin > FeFin))
                                   select new Agenda()
                                   {
                                       Curso = c.NombreCurso,
                                       FechaInicio = a.FechaInicio,
                                       FechaFin = a.FechaFin,
                                       Color = d.Valor2,
                                       CursoProgramadoId = a.CursoProgramadoId
                                   }).ToList();


                return return_data;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public ProgramacionCursos GetCalendarEvent(int id)
        {
            try
            {
                int NoTemporal = (int)Enumeradores.RecordType.NoTemporal;
                int Grabado = (int)Enumeradores.RecordStatus.Grabado;

                ProgramacionCursos return_data = (from a in ctx.CursosProgramados
                                                  join b in ctx.Eventos on a.EventoId equals b.EventoId
                                                  select new ProgramacionCursos()
                                                  {
                                                      RecordType = NoTemporal,
                                                      RecordStatus = Grabado,
                                                      CursoProgramadoId = a.CursoProgramadoId,
                                                      FechaInicio = a.FechaInicio,
                                                      FechaFin = a.FechaFin,
                                                      SedeId = b.SedeId,
                                                      EventoId = a.EventoId,
                                                      CursoId = a.CursoId
                                                  }).FirstOrDefault();

                return return_data;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
