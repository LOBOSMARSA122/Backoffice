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
    public class RegistroNotasRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public BandejaRegistroNotas BandejaRegistroNotas(BandejaRegistroNotas data)
        {
            try
            {
                int skip = (data.Index - 1) * data.Take;
                var query = (from a in ctx.CursosProgramados
                             join b in ctx.SalonProgramados on a.CursoProgramadoId equals b.CursoProgramadoId
                             join c in ctx.Eventos on a.EventoId equals c.EventoId
                             join d in ctx.Parametros on new {a = c.SedeId, b = 106} equals new {a = d.ParametroId, b = d.GrupoId }
                             join e in ctx.Cursos on a.CursoId equals e.CursoId
                             join f in ctx.Capacitadores on b.CapacitadorId equals f.CapacitadorId
                             join g in ctx.Personas on f.PersonaId equals g.PersonaId
                             where  (data.capacitadorId == -1 || b.CapacitadorId == data.capacitadorId) &&
                                    (data.cursoId == -1 || a.CursoId == data.cursoId) &&
                                    a.EsEliminado == 0
                             select new RegistroNotasList()
                             {
                                 sedeId = c.EventoId,
                                 sede = d.Valor1,
                                 eventoId = c.EventoId,
                                 evento = c.Nombre,
                                 cursoId = a.CursoId,
                                 curso = e.NombreCurso,
                                 CapacitadorId = f.CapacitadorId,
                                 Capacitador = g.Nombres + " " + g.ApellidoPaterno + " " + g.ApellidoMaterno,

                             }).ToList();
                int TotalRegistros = query.Count;
                query = query.Skip(skip).Take(data.Take).ToList();
                BandejaRegistroNotas returnData = new BandejaRegistroNotas()
                {
                    Lista = query,
                    TotalRegistros = TotalRegistros
                };

                return returnData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
