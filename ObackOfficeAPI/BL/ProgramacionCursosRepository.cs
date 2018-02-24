using System;
using BE.Administracion;
using BE.Comun;
using System.Collections.Generic;
using System.Linq;
using DAL;
using System.Globalization;

namespace BL
{
    public class ProgramacionCursosRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Agenda> GetAllAgenda(int SedeId, int EventoId, int CursoId, int year, int month)
        {
            try
            {
                int EsNoEliminado = (int)Enumeradores.EsEliminado.No;
                DateTime FeIni = new DateTime(year, month, 1);
                DateTime FeFin = new DateTime(year, month, 1).AddMonths(1);

                var return_data = (from a in ctx.CursosProgramados
                                   join b in ctx.Eventos on a.EventoId equals b.EventoId
                                   join c in ctx.Cursos on a.CursoId equals c.CursoId
                                   join d in ctx.Parametros on new { a = c.ColorId, b = 108 } equals new { a = d.ParametroId, b = d.GrupoId }
                                   where
                                   (SedeId == -1 || b.SedeId == SedeId) &&
                                   (EventoId == -1 || b.EventoId == EventoId) &&
                                   (CursoId == -1 || c.CursoId == CursoId) &&
                                   ((a.FechaInicio > FeIni && a.FechaFin < FeFin) ||
                                   (a.FechaInicio < FeIni && a.FechaFin > FeIni) ||
                                   (a.FechaInicio < FeFin && a.FechaFin > FeFin)) &&
                                   a.EsEliminado == EsNoEliminado
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
                int EsNoEliminado = (int)Enumeradores.EsEliminado.No;
                int NoTemporal = (int)Enumeradores.RecordType.NoTemporal;
                int Grabado = (int)Enumeradores.RecordStatus.Grabado;

                ProgramacionCursos return_data = (from a in ctx.CursosProgramados
                                                  join b in ctx.Eventos on a.EventoId equals b.EventoId
                                                  join c in ctx.SalonProgramados on a.CursoProgramadoId equals c.CursoProgramadoId
                                                  join d in ctx.SalonClases on c.SalonProgramadoId equals d.SalonProgramadoId
                                                  where a.EsEliminado == EsNoEliminado && d.EsEliminado == EsNoEliminado &&
                                                    a.CursoProgramadoId == id
                                                  group new { a,b,c,d } by a.CursoProgramadoId into curso
                                                  select new ProgramacionCursos()
                                                  {
                                                      RecordType = NoTemporal,
                                                      RecordStatus = Grabado,
                                                      CursoProgramadoId = curso.FirstOrDefault().a.CursoProgramadoId,
                                                      FechaInicio = curso.FirstOrDefault().a.FechaInicio,
                                                      FechaFin = curso.FirstOrDefault().a.FechaFin,
                                                      SedeId = curso.FirstOrDefault().b.SedeId,
                                                      EventoId = curso.FirstOrDefault().a.EventoId,
                                                      CursoId = curso.FirstOrDefault().a.CursoId,
                                                      Salones = curso.GroupBy(y => y.c.SalonProgramadoId)
                                                      .Select(x => new ProgramacionCursosSalones() {
                                                          RecordType = NoTemporal,
                                                          RecordStatus = Grabado,
                                                          CapacitadorId = x.FirstOrDefault().c.CapacitadorId,
                                                          SalonId = x.FirstOrDefault().c.SalonProgramadoId,
                                                          Cupos = x.FirstOrDefault().c.NroCupos,
                                                          EsEliminado = x.FirstOrDefault().c.EsEliminado,
                                                          Clases = x.Select( z => new ProgramacionCursosClases() {
                                                              RecordType = NoTemporal,
                                                              RecordStatus = Grabado,
                                                              ClaseId = z.d.SalonClaseId,
                                                              HoraInicio = z.d.FechaInicio,
                                                              HoraFin = z.d.FechaFin,
                                                              EsEliminado = z.d.EsEliminado
                                                          }).ToList()
                                                      }).ToList()
                                                  }).FirstOrDefault();

                return_data.Salones = (from a in return_data.Salones
                                       select new ProgramacionCursosSalones()
                                       {
                                           RecordType = NoTemporal,
                                           RecordStatus = Grabado,
                                           CapacitadorId = a.CapacitadorId,
                                           SalonId = a.SalonId,
                                           Cupos = a.Cupos,
                                           EsEliminado = a.EsEliminado,
                                           Clases = a.Clases.Select(z => new ProgramacionCursosClases()
                                           {
                                               RecordType = z.RecordType,
                                               RecordStatus = z.RecordStatus,
                                               ClaseId = z.ClaseId,
                                               HoraInicio = DateTime.SpecifyKind(z.HoraInicio,DateTimeKind.Utc),
                                               HoraFin = DateTime.SpecifyKind(z.HoraFin,DateTimeKind.Utc),
                                               EsEliminado = z.EsEliminado
                                           }).ToList()
                                       }).ToList();

                return return_data;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public bool CursoDataProcess(ProgramacionCursos data)
        {
            try
            {
                switch (data.RecordStatus)
                {
                    case (int)Enumeradores.RecordStatus.Agregar:
                        {
                            return SaveNewCursoAndChildren(data);
                        }
                    case (int)Enumeradores.RecordStatus.Editar:
                    case (int)Enumeradores.RecordStatus.Grabado:
                        {
                            return CursoAndChildrenPorStatus(data);
                        }
                    case (int)Enumeradores.RecordStatus.Eliminar:
                        {
                            return DeleteCursoAndChildren(data);
                        }
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool SaveNewCursoAndChildren(ProgramacionCursos data)
        {
            int EsNoEliminado = (int)Enumeradores.EsEliminado.No;
            try
            {
                ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                var salonesID = (from a in ctx.EventoSalones where a.EventoId == data.EventoId select a).ToList();

                CursoProgramado Curso = new CursoProgramado()
                {
                    CursoId = data.CursoId,
                    EsEliminado = EsNoEliminado,
                    EventoId = data.EventoId,
                    FechaInicio = data.FechaInicio,
                    FechaFin = data.FechaFin,
                    UsuGraba = data.UsuarioActualizaID,
                    FechaGraba = DateTime.Now
                };
                ctx.CursosProgramados.Add(Curso);
                ctx.SaveChanges();

                if (Curso.CursoProgramadoId == 0)
                {
                    throw new Exception();
                }
                else
                {
                    int contadorSalon = 0;
                    foreach (var S in data.Salones)
                    {
                        contadorSalon++;
                        SalonProgramado Salon = new SalonProgramado()
                        {
                            CapacitadorId = S.CapacitadorId,
                            CursoProgramadoId = Curso.CursoProgramadoId,
                            EsEliminado = EsNoEliminado,
                            EventoSalonId = salonesID.Where(x => x.Nombre.Contains(contadorSalon.ToString())).FirstOrDefault().EventoSalonId,
                            NroCupos = S.Cupos,
                            UsuGraba = data.UsuarioActualizaID,
                            FechaGraba = DateTime.Now
                        };

                        ctx.SalonProgramados.Add(Salon);
                        ctx.SaveChanges();

                        if(Salon.SalonProgramadoId == 0)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            foreach(var C in S.Clases)
                            {
                                SalonClases Clase = new SalonClases()
                                {
                                    EsEliminado = EsNoEliminado,
                                    FechaInicio = C.HoraInicio,
                                    FechaFin = C.HoraFin,
                                    SalonProgramadoId = Salon.SalonProgramadoId,
                                    UsuGraba = data.UsuarioActualizaID,
                                    FechaGraba = DateTime.Now
                                };

                                ctx.SalonClases.Add(Clase);
                            }
                            ctx.SaveChanges();
                        }
                    }
                }

                ctx.Database.CurrentTransaction.Commit();
                return true;
            }
            catch(Exception e)
            {
                ctx.Database.CurrentTransaction.Rollback();
                return false;
            }
        }

        public bool DeleteCursoAndChildren(ProgramacionCursos data)
        {
            int EsEliminado = (int)Enumeradores.EsEliminado.Si;
            try
            {
                ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                var Curso = (from a in ctx.CursosProgramados where a.CursoProgramadoId == data.CursoProgramadoId select a).FirstOrDefault();

                if (Curso == null)
                {
                    throw new Exception();
                }
                else
                {
                    var Salones = (from a in ctx.SalonProgramados where a.CursoProgramadoId == Curso.CursoProgramadoId select a).ToList();

                    if(Salones == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        foreach(var S in Salones)
                        {
                            var Clases = (from a in ctx.SalonClases where a.SalonProgramadoId == S.SalonProgramadoId select a).ToList();

                            if(Clases == null)
                            {
                                throw new Exception();
                            }
                            else
                            {
                                foreach(var C in Clases)
                                {
                                    C.EsEliminado = EsEliminado;
                                    C.UsuActualiza = data.UsuarioActualizaID;
                                    C.FechaActualiza = DateTime.Now;
                                }
                            }
                            S.EsEliminado = EsEliminado;
                            S.UsuActualiza = data.UsuarioActualizaID;
                            S.FechaActualiza = DateTime.Now;
                        }
                    }

                    Curso.EsEliminado = EsEliminado;
                    Curso.UsuActualiza = data.UsuarioActualizaID;
                    Curso.FechaActualiza = DateTime.Now;
                }

                ctx.SaveChanges();
                ctx.Database.CurrentTransaction.Commit();
                return true;
            }
            catch(Exception e)
            {
                ctx.Database.CurrentTransaction.Rollback();
                return false;
            }
        }

        public bool CursoAndChildrenPorStatus(ProgramacionCursos data)
        {
            try
            {
                int EsNoEliminado = (int)Enumeradores.EsEliminado.No;
                int EsEliminado = (int)Enumeradores.EsEliminado.Si;

                ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                var salonesID = (from a in ctx.EventoSalones where a.EventoId == data.EventoId select a).ToList();

                if (data.RecordStatus == (int)Enumeradores.RecordStatus.Editar)
                {
                    var Curso = (from a in ctx.CursosProgramados where a.CursoProgramadoId == data.CursoProgramadoId select a).FirstOrDefault();
                    Curso.CursoId = data.CursoId;
                    Curso.EventoId = data.EventoId;
                    Curso.FechaActualiza = DateTime.Now;
                    Curso.FechaFin = data.FechaFin;
                    Curso.FechaInicio = data.FechaInicio;
                    Curso.UsuActualiza = data.UsuarioActualizaID;
                    ctx.SaveChanges();
                }

                int contadorSalon = 0;
                foreach (var S in data.Salones)
                {
                    contadorSalon++;
                    switch (S.RecordStatus)
                    {
                        case (int)Enumeradores.RecordStatus.Agregar:
                            {
                                SalonProgramado Salon = new SalonProgramado()
                                {
                                    CapacitadorId = S.CapacitadorId,
                                    CursoProgramadoId = data.CursoProgramadoId,
                                    EsEliminado = EsNoEliminado,
                                    EventoSalonId = salonesID.Where(x => x.Nombre.Contains(contadorSalon.ToString())).FirstOrDefault().EventoSalonId,
                                    NroCupos = S.Cupos,
                                    UsuGraba = data.UsuarioActualizaID,
                                    FechaGraba = DateTime.Now
                                };

                                ctx.SalonProgramados.Add(Salon);
                                ctx.SaveChanges();

                                if (Salon.SalonProgramadoId == 0)
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    foreach (var C in S.Clases)
                                    {
                                        SalonClases Clase = new SalonClases()
                                        {
                                            EsEliminado = EsNoEliminado,
                                            FechaInicio = C.HoraInicio,
                                            FechaFin = C.HoraFin,
                                            SalonProgramadoId = Salon.SalonProgramadoId,
                                            UsuGraba = data.UsuarioActualizaID,
                                            FechaGraba = DateTime.Now
                                        };

                                        ctx.SalonClases.Add(Clase);
                                    }
                                    ctx.SaveChanges();
                                }
                                break;
                            }
                        case (int)Enumeradores.RecordStatus.Eliminar:
                            {
                                var SA = (from a in ctx.SalonProgramados where a.SalonProgramadoId == S.SalonId select a).FirstOrDefault();
                                var Clases = (from a in ctx.SalonClases where a.SalonProgramadoId == SA.SalonProgramadoId select a).ToList();

                                if (Clases == null)
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    foreach (var C in Clases)
                                    {
                                        C.EsEliminado = EsEliminado;
                                        C.UsuActualiza = data.UsuarioActualizaID;
                                        C.FechaActualiza = DateTime.Now;
                                    }
                                }
                                SA.EsEliminado = EsEliminado;
                                SA.UsuActualiza = data.UsuarioActualizaID;
                                SA.FechaActualiza = DateTime.Now;
                                ctx.SaveChanges();
                                break;
                            }
                        case (int)Enumeradores.RecordStatus.Grabado:
                        case (int)Enumeradores.RecordStatus.Editar:
                            {
                                if(S.RecordStatus == (int)Enumeradores.RecordStatus.Editar)
                                {
                                    var SA = (from a in ctx.SalonProgramados where a.SalonProgramadoId == S.SalonId select a).FirstOrDefault();
                                    SA.CapacitadorId = S.CapacitadorId;
                                    SA.FechaActualiza = DateTime.Now;
                                    SA.NroCupos = S.Cupos;
                                    SA.UsuActualiza = data.UsuarioActualizaID;
                                    ctx.SaveChanges();
                                }

                                foreach(var C in S.Clases)
                                {
                                    switch (C.RecordStatus)
                                    {
                                        case (int)Enumeradores.RecordStatus.Agregar:
                                            {
                                                SalonClases Clase = new SalonClases()
                                                {
                                                    EsEliminado = EsNoEliminado,
                                                    FechaInicio = C.HoraInicio,
                                                    FechaFin = C.HoraFin,
                                                    SalonProgramadoId = S.SalonId,
                                                    UsuGraba = data.UsuarioActualizaID,
                                                    FechaGraba = DateTime.Now
                                                };

                                                ctx.SalonClases.Add(Clase);
                                                ctx.SaveChanges();
                                                break;
                                            }
                                        case (int)Enumeradores.RecordStatus.Eliminar:
                                            {
                                                var CA = (from a in ctx.SalonClases where a.SalonClaseId == C.ClaseId select a).FirstOrDefault();
                                                CA.EsEliminado = EsEliminado;
                                                CA.UsuActualiza = data.UsuarioActualizaID;
                                                CA.FechaActualiza = DateTime.Now;
                                                ctx.SaveChanges();
                                                break;
                                            }
                                        case (int)Enumeradores.RecordStatus.Editar:
                                            {
                                                var CA = (from a in ctx.SalonClases where a.SalonClaseId == C.ClaseId select a).FirstOrDefault();
                                                CA.FechaActualiza = DateTime.Now;
                                                CA.FechaFin = C.HoraFin;
                                                CA.FechaInicio = C.HoraInicio;
                                                CA.UsuActualiza = data.UsuarioActualizaID;
                                                ctx.SaveChanges();
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                    }
                }


                ctx.SaveChanges();
                ctx.Database.CurrentTransaction.Commit();
                return true;
            }
            catch(Exception e)
            {
                ctx.Database.CurrentTransaction.Rollback();
                return false;
            }
        }
    }
}
