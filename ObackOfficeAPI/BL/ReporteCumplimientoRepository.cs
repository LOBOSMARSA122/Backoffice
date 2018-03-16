using DAL;
using BE.Comun;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class ReporteCumplimientoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public BandejaReporteCumplimiento BandejaReporteCumplimiento(BandejaReporteCumplimiento data)
        {
            try
            {
                int NoEsEliminado = (int)Enumeradores.EsEliminado.No;
                int AprobadoId = (int)Enumeradores.Condicion.Aprobado;

                List<int> ListaCursoId = new List<int>() {7,3,8,12,1,9,26};

                var Listado = (from a in ctx.Cursos
                              join b in
                              (from a in ctx.Personas
                               join b in ctx.Empleados on a.PersonaId equals b.PersonaId
                               join EC in ctx.EmpleadoCursos on b.EmpleadoId equals EC.EmpleadoId into ECG
                               from c in ECG.DefaultIfEmpty()
                               join SP in ctx.SalonProgramados on c.SalonProgramadoId equals SP.SalonProgramadoId into SPG
                               from d in SPG.DefaultIfEmpty()
                               join CP in ctx.CursosProgramados on d.CursoProgramadoId equals CP.CursoProgramadoId into CPG
                               from e in CPG.DefaultIfEmpty()
                               select new
                               {
                                   a.PersonaId,
                                   c.CondicionId,
                                   e.CursoId,
                                   b.Area
                               }) on a.CursoId equals b.CursoId into joined
                              from j in joined.DefaultIfEmpty()
                              select new
                              {
                                  a.CursoId,
                                  a.NombreCurso,
                                  j.CondicionId,
                                  j.PersonaId,
                                  j.Area
                              }).ToList();

                data.Lista = (from a in Listado
                          group a by a.Area into grp
                          select new ReporteCumplimientoList()
                          {
                              Area = grp.Key,
                              Total = (from b in grp group b by b.PersonaId into z select z.Count()).Sum(),
                              ExcavacionesSubterraneas = (from b in grp where b.CursoId == 7 && b.CondicionId == AprobadoId group b by b.CursoId into z select z.Count()).Sum(),
                              HerramientasManuales = (from b in grp where b.CursoId == 3 && b.CondicionId == AprobadoId group b by b.CursoId into z select z.Count()).Sum(),
                              PrevencionDeCaidas = (from b in grp where b.CursoId == 8 && b.CondicionId == AprobadoId group b by b.CursoId into z select z.Count()).Sum(),
                              VehiculosEquiposMoviles = (from b in grp where b.CursoId == 12 && b.CondicionId == AprobadoId group b by b.CursoId into z select z.Count()).Sum(),
                              AislamientoBloqueoDeEnergia = (from b in grp where b.CursoId == 1 && b.CondicionId == AprobadoId group b by b.CursoId into z select z.Count()).Sum(),
                              SistemasPresurizados = (from b in grp where b.CursoId == 9 && b.CondicionId == AprobadoId group b by b.CursoId into z select z.Count()).Sum(),
                              RiesgosCriticosGeneral = (from b in grp where b.CursoId == 26 && b.CondicionId == AprobadoId group b by b.CursoId into z select z.Count()).Sum(),
                          }).ToList();


                return data;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
