using BE.Administracion;
using BE.RegistroNotas;
using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
                                 cursoProgramadoId = a.CursoProgramadoId,
                                 salonProgramadoId = b.SalonProgramadoId,
                                 sedeId = c.EventoId,
                                 sede = d.Valor1,
                                 eventoId = c.EventoId,
                                 evento = c.Nombre,
                                 cursoId = a.CursoId,
                                 curso = e.NombreCurso,
                                 CapacitadorId = f.CapacitadorId,
                                 Capacitador = g.Nombres + " " + g.ApellidoPaterno + " " + g.ApellidoMaterno,

                             }).ToList();
                data.TotalRegistros = query.Count;
                if (data.Take > 0)
                    query = query.Skip(skip).Take(data.Take).ToList();

                data.Lista = query;

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RegistroNotas> GetRegistroNotas(int salonProgramadoId)
        {
            try
            {
                int Grabado = (int)Enumeradores.RecordStatus.Grabado;
                var query = (from a in ctx.EmpleadoCursos
                             join b in ctx.EmpleadoAsistencias on a.EmpleadoCursoId equals b.EmpleadoCursoId
                             join c in ctx.Empleados on a.EmpleadoId equals c.EmpleadoId
                             join d in ctx.Personas on c.PersonaId equals d.PersonaId
                             join f in ctx.EmpleadoTalleres on a.EmpleadoCursoId equals f.EmpleadoCursoId
                             join fp in ctx.Parametros on new { a = f.PreguntaId, b = 105 } equals new { a = fp.ParametroId, b = fp.GrupoId }
                             join g in ctx.SalonProgramados on a.SalonProgramadoId equals g.SalonProgramadoId
                             join h in ctx.Capacitadores on g.CapacitadorId equals h.CapacitadorId
                             join i in ctx.Personas on h.PersonaId equals i.PersonaId
                             join j in ctx.CursosProgramados on g.CursoProgramadoId equals j.CursoProgramadoId
                             join k in ctx.Cursos on j.CursoId equals k.CursoId
                             join l in ctx.Empresas on c.EmpresaId equals l.EmpresaId
                             where a.SalonProgramadoId == salonProgramadoId
                             //group new { a, b, d, f,i,g,j,k,fp } by new { a.EmpleadoId, a.SalonProgramadoId } into grp
                             select new 
                             {
                                 Capacitador = i.Nombres + " " + i.ApellidoPaterno + " " + i.ApellidoMaterno,
                                 FechaInicioCurso = j.FechaInicio,
                                 FechaFinCurso = j.FechaFin,
                                 NroCupos = g.NroCupos,
                                 Curso = k.NombreCurso,
                                 EmpleadoCursoId = a.EmpleadoCursoId,
                                 SalonProgramadoId = a.SalonProgramadoId,
                                 EmpleadoId = c.EmpleadoId,
                                 PersonaId = d.PersonaId,
                                 NombreCompletoEmpleado = d.Nombres + " " + d.ApellidoPaterno + " " + d.ApellidoMaterno,
                                 Nota = a.Nota,
                                 Taller = a.NotaTaller,
                                 CondicionId = a.CondicionId,
                                 Observacion = a.Observacion,
                                 RecordStatus = Grabado,
                                 EmpleadoAsistenciaId = b.EmpleadoAsistenciaId,
                                 FechaClase = b.FechaClase,
                                 Asistio = b.Asistio,
                                 EmpleadoTallerId = f.EmpleadoTallerId,
                                 PreguntaId = f.PreguntaId,
                                 Pregunta = fp.Valor1,
                                 Valor = f.Valor,
                                 Empresa =  l.RazonSocial
                             }
                             ).ToList();

                var queryGroup = query.GroupBy(g => new { g.EmpleadoId, g.SalonProgramadoId })
                                .Select(s => s.First())
                                .OrderBy(o => o.EmpleadoCursoId).ToList();

                List<RegistroNotas> result = new List<RegistroNotas>();

                result = queryGroup
                        .Select(x => new RegistroNotas
                        {
                            Capacitador = x.Capacitador,
                            FechaInicioCurso = x.FechaInicioCurso,
                            FechaFinCurso = x.FechaFinCurso,
                            NroCupos = x.NroCupos,
                            Curso = x.Curso,
                            Empresa = x.Empresa,

                            EmpleadoCursoId = x.EmpleadoCursoId,
                            SalonProgramadoId = x.SalonProgramadoId,
                            EmpleadoId = x.EmpleadoId,
                            PersonaId = x.PersonaId,
                            NombreCompletoEmpleado = x.NombreCompletoEmpleado,
                            Nota = x.Nota,
                            Taller = x.Taller,
                            CondicionId = x.CondicionId,
                            Observacion = x.Observacion,
                            RecordStatus = Grabado
                        }).ToList();

                foreach (var item in result)
                {
                    item.EmpleadoAsistencia = new List<Asistencia>();
                    //Lista EmpleadoAsistencia por Alumno
                    IEnumerable<Asistencia> lAsistencia = query.FindAll(p => p.EmpleadoCursoId == item.EmpleadoCursoId)
                                    .Select(x => new Asistencia
                                    {
                                        EmpleadoAsistenciaId = x.EmpleadoAsistenciaId,
                                        EmpleadoCursoId = x.EmpleadoCursoId,
                                        FechaClase = x.FechaClase,
                                        Asistio = x.Asistio,
                                        RecordStatus = Grabado
                                    }).ToList();
                    item.EmpleadoAsistencia.AddRange(lAsistencia.GroupBy(g => g.EmpleadoAsistenciaId).Select(f => f.First()).ToList());

                    //Lista EmpleadoTaller por Alumno
                    item.EmpleadoTaller = new List<Taller>();
                    IEnumerable<Taller> lTaller = query.FindAll(p => p.EmpleadoCursoId == item.EmpleadoCursoId)
                                   .Select(x => new Taller
                                   {
                                       EmpleadoTallerId = x.EmpleadoTallerId,
                                       EmpleadoCursoId = x.EmpleadoCursoId,
                                       PreguntaId = x.PreguntaId,
                                       Pregunta = x.Pregunta,
                                       Valor = x.Valor,
                                       RecordStatus = Grabado
                                   }).ToList();
                    item.EmpleadoTaller.AddRange(lTaller.GroupBy(g => g.EmpleadoTallerId).Select(f => f.First()).ToList());

                }
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool GrabarRegistro(List<RegistroNotas> data, int UsuId)
        {
            try
            {
                ctx.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
                //Buscar todos los registros que tienen el flag actualizado
                var listaEmpleadoCurso = data.FindAll(p => p.RecordStatus == (int)Enumeradores.RecordStatus.Editar).ToList();
                var ListaEmpleadoTaller = data.SelectMany(p => p.EmpleadoTaller).Where(x => x.RecordStatus == (int)Enumeradores.RecordStatus.Editar).ToList();


                //Actualizar en TblEmpleadoCurso
                foreach (var EmpleadoCurso in listaEmpleadoCurso)
                {
                    var oEmpleadoCurso = (from a in ctx.EmpleadoCursos where a.EmpleadoCursoId == EmpleadoCurso.EmpleadoCursoId select a).FirstOrDefault();

                    oEmpleadoCurso.Nota = EmpleadoCurso.Nota;
                    oEmpleadoCurso.CondicionId = EmpleadoCurso.CondicionId;
                    oEmpleadoCurso.Observacion = EmpleadoCurso.Observacion;
                    oEmpleadoCurso.NotaTaller = "Iniciado";
                    oEmpleadoCurso.UsuActualiza = UsuId;
                    oEmpleadoCurso.FechaActualiza = DateTime.Now;

                    //Actualizar en TblEmpleadoAsistencia
                    foreach (var Asistencia in EmpleadoCurso.EmpleadoAsistencia)
                    {
                        var oEmpleadoAsistencia = (from a in ctx.EmpleadoAsistencias where a.EmpleadoAsistenciaId == Asistencia.EmpleadoAsistenciaId select a).FirstOrDefault();

                        oEmpleadoAsistencia.Asistio = Asistencia.Asistio.Value;
                        oEmpleadoAsistencia.UsuActualiza = UsuId;
                        oEmpleadoAsistencia.FechaActualiza = DateTime.Now;
                    }
                }

                //Actualizar en EmpleadoTaller

                foreach (var EmpleadoTaller in ListaEmpleadoTaller)
                {
                    var oEmpleadoTaller = (from a in ctx.EmpleadoTalleres where a.EmpleadoTallerId == EmpleadoTaller.EmpleadoTallerId select a).FirstOrDefault();

                    oEmpleadoTaller.Valor = EmpleadoTaller.Valor;
                    oEmpleadoTaller.UsuActualiza = UsuId;
                    oEmpleadoTaller.FechaActualiza = DateTime.Now;
                }

                ctx.SaveChanges();

                ctx.Database.CurrentTransaction.Commit();

                return true;
            }
            catch (Exception)
            {
                ctx.Database.CurrentTransaction.Rollback();
                throw;
            }
        }

        public List<string> CargaExamenDiploma(Dictionary<string, byte[]> lista,string directorioExamenes, string directorioDiplomas)
        {
            List<string> return_data = new List<string>();
            foreach (var Archivo in lista)
            {
                try
                {
                    string path = "";

                    switch (Archivo.Key.Split('-')[0])
                    {
                        case "E":
                            {
                                path = directorioExamenes;
                                break;
                            }
                        case "D":
                            {
                                path = directorioDiplomas;
                                break;
                            }
                        default:
                            {
                                throw new Exception();
                            }
                    }

                    path += Archivo.Key;

                    File.WriteAllBytes(path, Archivo.Value);
                    
                }
                catch (Exception e)
                {
                    return_data.Add(Archivo.Key);
                }
            }
            return return_data;
        }
    }
}
