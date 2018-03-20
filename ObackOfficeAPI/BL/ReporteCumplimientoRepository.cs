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

                int EmpresaId = 0;
                if (!string.IsNullOrWhiteSpace(data.Empresa))
                {
                    EmpresaId = (from a in ctx.Empresas where a.RazonSocial == data.Empresa select a.EmpresaId).FirstOrDefault();
                }

                List<int> ListaCursoId = new List<int>() {7,3,8,12,1,9,26};

                var Listado = (from a in ctx.Cursos
                               join b in
                               (from a in ctx.Personas

                                join b in ctx.Empleados on a.PersonaId equals b.PersonaId

                                join EC in ctx.EmpleadoCursos on b.EmpleadoId equals EC.EmpleadoId

                                join SP in ctx.SalonProgramados on EC.SalonProgramadoId equals SP.SalonProgramadoId

                                join CP in ctx.CursosProgramados on SP.CursoProgramadoId equals CP.CursoProgramadoId
                                select new
                                {
                                    a.PersonaId,
                                    EC.CondicionId,
                                    CP.CursoId,
                                    b.Area,
                                    b.EmpresaId
                                }) on a.CursoId equals b.CursoId
                               where (EmpresaId == 0 || b.EmpresaId == EmpresaId)
                               select new
                               {
                                   a.CursoId,
                                   a.NombreCurso,
                                   b.CondicionId,
                                   b.PersonaId,
                                   b.Area
                               }).ToList();

                data = new BandejaReporteCumplimiento();

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
