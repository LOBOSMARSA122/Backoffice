using System;
using System.Collections.Generic;
using System.Linq;
using BE.Comun;
using BE.Acceso;
using BE.Cliente;
using BE.Administracion;
using BE.RegistroNotas;
using BE;
using DAL;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BL
{
    public class PersonRepository
    {
        private List<Person> _lPerson = new List<Person>();
        private DatabaseContext ctx = new DatabaseContext();

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Person> GetAll()
        {
            try
            {
                var query = (from a in ctx.People select a).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public Person GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save(Person oPerson)
        {
            throw new NotImplementedException();
        }

        public List<Dropdownlist> GetGeneros()
        {
            int grupo = (int)Enumeradores.GrupoParametros.Generos;
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Dropdownlist> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo orderby a.Orden ascending
                                        select new Dropdownlist() {
                                            Id = a.ParametroId,
                                            Value = a.Valor1
                                        }).ToList();

            return result;
        }

        public Parametro InsertGenero(string Descripcion, int UsuarioID)
        {
            try
            {
                int grupo = (int)Enumeradores.GrupoParametros.Generos;
                int NoEliminado = (int)Enumeradores.EsEliminado.No;

                List<Parametro> Listado = (from a in ctx.Parametros where a.GrupoId == grupo select a).ToList();
                int parametroId = (from a in Listado orderby a.ParametroId descending select a.ParametroId).FirstOrDefault() + 1;
                int orden = (from a in Listado orderby a.Orden descending select a.Orden).FirstOrDefault() + 1;

                Parametro Parametro = (from a in Listado where a.Valor1 == Descripcion select a).FirstOrDefault();

                if(Parametro != null)
                    return null;

                Parametro = new Parametro()
                {
                    GrupoId = grupo,
                    ParametroId = parametroId,
                    Valor1 = Descripcion,
                    PadreParametroId = -1,
                    Orden = orden,
                    EsEliminado = NoEliminado,
                    FechaGraba = DateTime.Now,
                    UsuGraba = UsuarioID
                };

                ctx.Parametros.Add(Parametro);

                int rows = ctx.SaveChanges();
                if (rows > 0)
                    return Parametro;

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Dropdownlist> GetRoles()
        {
            int grupo = (int)Enumeradores.GrupoParametros.Roles;
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Dropdownlist> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo orderby a.Orden ascending
                                      select new Dropdownlist()
                                      {
                                          Id = a.ParametroId,
                                          Value = a.Valor1
                                      }).ToList();

            return result;
        }

        public bool InsertNewPerson(Persona Persona, Usuario Usuario, int UsuarioID)
        {
            try
            {
                if ((from a in ctx.Personas where a.TipoDocumentoId == Persona.TipoDocumentoId && a.NroDocumento == Persona.NroDocumento select a).FirstOrDefault() != null || (from a in ctx.Usuarios where a.NombreUsuario == Usuario.NombreUsuario select a).FirstOrDefault() != null)
                    return false;

                int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

                Persona.UsuGraba = UsuarioID;
                Persona.FechaGraba = DateTime.Now;
                Persona.EsEliminado = NoEsEliminado;
                Persona.GeneroId = Persona.GeneroId == -1 ? 0 : Persona.GeneroId;

                ctx.Personas.Add(Persona);
                int rows = ctx.SaveChanges();

                string pass = Usuario.Contrasenia;

                Usuario.UsuGraba = UsuarioID;
                Usuario.FechaGraba = DateTime.Now;
                Usuario.EsEliminado = NoEsEliminado;
                Usuario.FechaCaduca = DateTime.Now.AddYears(1);
                Usuario.Contrasenia = Utils.Encrypt(Usuario.Contrasenia);
                Usuario.PersonaId = Persona.PersonaId;

                ctx.Usuarios.Add(Usuario);

                rows += ctx.SaveChanges();

                int grupoEmail = (int)Enumeradores.GrupoParametros.Correo;
                int parametroBody = (int)Enumeradores.Correo.MailRegistroEmpleado;
                int parametroCorreo = (int)Enumeradores.Correo.CorreoSistema;
                int parametroClave = (int)Enumeradores.Correo.ClaveCorreo;
                int parametroHost = (int)Enumeradores.Correo.HostSMTP;

                var parametros = (from a in ctx.Parametros where a.GrupoId == grupoEmail select a).ToList();

                string CorreoSistema = (from a in parametros where a.ParametroId == parametroCorreo select a.Valor2).FirstOrDefault();
                string ClaveCorreo = (from a in parametros where a.ParametroId == parametroClave select a.Valor2).FirstOrDefault();
                string CorreoHost = (from a in parametros where a.ParametroId == parametroHost select a.Valor2).FirstOrDefault();

                string body = (from a in parametros where a.ParametroId == parametroBody select a.Campo).FirstOrDefault();

                body = body.Replace("[@NOMBRE_PERSONA@]", string.Format("{0} {1} {2}", Persona.Nombres, Persona.ApellidoPaterno, Persona.ApellidoMaterno)).Replace("[@NOMBRE_USUARIO@]", Usuario.NombreUsuario).Replace("[@PASSWORD@]", pass);

                string subject = "Registro Exitoso de Usuario";
                List<string> adresses = new List<string>();
                adresses.Add(Persona.CorreoElectronico);

                Task TASK = new Task(() => Utils.SendSimpleMail(body, subject, adresses, CorreoSistema, ClaveCorreo, CorreoHost));
                TASK.Start();

                if (rows > 1)
                    return true;

                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool EditPerson(Persona Persona, Usuario Usuario, int UsuarioID)
        {
            try
            {
                var ctxPersona = (from a in ctx.Personas where a.PersonaId == Persona.PersonaId select a).FirstOrDefault();
                var ctxUsuario = (from a in ctx.Usuarios where a.UsuarioId == Usuario.UsuarioId select a).FirstOrDefault();

                if (ctxPersona == null || ctxUsuario == null)
                    return false;

                ctxPersona.TipoDocumentoId = Persona.TipoDocumentoId;
                ctxPersona.NroDocumento = Persona.NroDocumento;
                ctxPersona.Nombres = Persona.Nombres;
                ctxPersona.ApellidoPaterno = Persona.ApellidoPaterno;
                ctxPersona.ApellidoMaterno = Persona.ApellidoMaterno;
                ctxPersona.FechaNacimiento = Persona.FechaNacimiento;
                ctxPersona.GeneroId = Persona.GeneroId == -1 ? 0 : Persona.GeneroId;
                ctxPersona.CorreoElectronico = Persona.CorreoElectronico;
                ctxPersona.NumeroCelular = Persona.NumeroCelular;
                ctxPersona.UsuActualiza = UsuarioID;
                ctxPersona.FechaActualiza = DateTime.Now;


                ctxUsuario.NombreUsuario = Usuario.NombreUsuario;
                if (Utils.Encrypt(Usuario.Contrasenia) != ctxUsuario.Contrasenia && !string.IsNullOrWhiteSpace(Usuario.Contrasenia))
                    ctxUsuario.Contrasenia = Utils.Encrypt(Usuario.Contrasenia);
                ctxUsuario.EmpresaId = Usuario.EmpresaId;
                ctxUsuario.RolId = Usuario.RolId;
                ctxUsuario.PreguntaSecreta = Usuario.PreguntaSecreta;
                if (Utils.Encrypt(Usuario.RespuestaSecreta) != ctxUsuario.RespuestaSecreta && !string.IsNullOrWhiteSpace(Usuario.RespuestaSecreta))
                    ctxUsuario.RespuestaSecreta = Utils.Encrypt(Usuario.RespuestaSecreta);
                ctxUsuario.UsuActualiza = UsuarioID;
                ctxUsuario.FechaActualiza = DateTime.Now;


                int rows = ctx.SaveChanges();

                if (rows > 1)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Dropdownlist> GetTipoDocumentos()
        {
            int grupo = (int)Enumeradores.GrupoParametros.TipoDocumentos;
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Dropdownlist> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo orderby a.Orden ascending
                                         select new Dropdownlist()
                                         {
                                             Id = a.ParametroId,
                                             Value = a.Valor1
                                         }).ToList();

            return result;
        }

        public Persona GetPersona(int id)
        {
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            return (from a in ctx.Personas where a.EsEliminado == NoEliminado && a.PersonaId == id select a).FirstOrDefault();
        }

        public MultiDataModel CargaMasivaArchivo(MemoryStream ms)
        {
            int RegistrosInsertados = 0;
            int RegistrosErrados = 0;
            try
            {
                IWorkbook book = new XSSFWorkbook(ms);
                ISheet Sheet = book.GetSheet(book.GetSheetName(0));

                int index = -1;
                bool indexEncontrado = false;

                while (!indexEncontrado)
                {
                    index++;
                    IRow Row = Sheet.GetRow(index);
                    if (Row != null)
                    {
                        ICell cell = Row.GetCell(1);
                        if (cell != null)
                        {
                            if (cell.StringCellValue == "APELLIDO PATERNO")
                            {
                                indexEncontrado = true;
                            }
                        }
                    }

                    if (index > 10)
                    {
                        throw new Exception();
                    }
                }

                int grupoTipoDocumentos = (int)Enumeradores.GrupoParametros.TipoDocumentos;
                int grupoTipoEmpresa = (int)Enumeradores.GrupoParametros.TipoEmpresas;
                int grupoCondicion = (int)Enumeradores.GrupoParametros.Condición;
                int NoEsEliminado = (int)Enumeradores.EsEliminado.No;
                int EsEliminado = (int)Enumeradores.EsEliminado.Si;

                var ListaTipoDocumentos = (from a in ctx.Parametros where a.GrupoId == grupoTipoDocumentos select new { Nombre = a.Valor1, Valor = a.ParametroId }).ToList();
                var ListaCondicion = (from a in ctx.Parametros where a.GrupoId == grupoCondicion select new { Nombre = a.Valor1, Valor = a.ParametroId }).ToList();
                
                var ListaCapacitadores = (from a in ctx.Capacitadores
                                          join b in ctx.Personas on a.PersonaId equals b.PersonaId
                                          select new { Nombre = b.Nombres + " " + b.ApellidoPaterno + " " + b.ApellidoMaterno, Valor = a.CapacitadorId }).ToList();
                
                var ListaTipoEmpresa = (from a in ctx.Parametros where a.GrupoId == grupoTipoEmpresa select new { Nombre = a.Valor1, Valor = a.ParametroId }).ToList();
                

                bool finArchivo = false;
                while (!finArchivo)
                {
                    try
                    {
                        var ListaEmpresas = (from a in ctx.Empresas select new { Nombre = a.RazonSocial, Valor = a.EmpresaId }).ToList();
                        var ListaSalones = (from a in ctx.EventoSalones select new { a.Nombre, Valor = a.EventoSalonId }).ToList();
                        var ListaCursos = (from a in ctx.Cursos select a).ToList();

                        index++;
                        IRow Row = Sheet.GetRow(index);
                        if (Row != null)
                        {
                            Persona P = new Persona();
                            Empleado E = new Empleado();
                            Curso C = new Curso();

                            P.ApellidoPaterno = Row.GetCell(1) != null ? Row.GetCell(1).ToString() : "";
                            P.ApellidoMaterno = Row.GetCell(2) != null ? Row.GetCell(2).ToString() : "";
                            P.Nombres = Row.GetCell(3) != null ? Row.GetCell(3).ToString() : "";
                            P.TipoDocumentoId = Row.GetCell(4) != null ? ListaTipoDocumentos.Where(x => x.Nombre.Contains(Row.GetCell(4).ToString().Split(' ')[0])).FirstOrDefault().Valor : 0;
                            P.NroDocumento = Row.GetCell(5) != null ? Row.GetCell(5).ToString() : "";
                            P.CorreoElectronico = Row.GetCell(8) != null ? Row.GetCell(8).ToString() : "";
                            P.NumeroCelular = Row.GetCell(9) != null ? Row.GetCell(9).ToString() : "";

                            Persona persona = (from a in ctx.Personas
                                               where a.TipoDocumentoId == P.TipoDocumentoId && a.NroDocumento == P.NroDocumento
                                               select a).FirstOrDefault();

                            if (persona == null)
                            {
                                persona = P;
                                persona.EsEliminado = NoEsEliminado;
                                persona.FechaGraba = DateTime.Now;

                                ctx.Personas.Add(persona);
                                ctx.SaveChanges();
                            }
                            

                            E.Cargo = Row.GetCell(6) != null ? Row.GetCell(6).ToString() : "";
                            E.Area = Row.GetCell(7) != null ? Row.GetCell(7).ToString() : "";

                            string nombreEmpresa = Row.GetCell(10) == null ? "" : Row.GetCell(10).ToString();
                            var empresa = ListaEmpresas.Where(x => x.Nombre.ToUpper() == nombreEmpresa.ToUpper()).FirstOrDefault();

                            if(empresa == null)
                            {
                                Empresa Empresa = new Empresa()
                                {
                                    EsEliminado = NoEsEliminado,
                                    FechaGraba = DateTime.Now,
                                    TipoEmpresaId = ListaTipoEmpresa.Where(x => x.Nombre == "Cliente").FirstOrDefault().Valor,
                                    RazonSocial = nombreEmpresa
                                };

                                ctx.Empresas.Add(Empresa);
                                ctx.SaveChanges();

                                E.EmpresaId = Empresa.EmpresaId;
                            }
                            else
                            {
                                E.EmpresaId = empresa.Valor;
                            }

                            Empleado empleado = (from a in ctx.Empleados
                                                 where a.PersonaId == persona.PersonaId && a.EmpresaId == E.EmpresaId
                                                 select a).FirstOrDefault();

                            if(empleado == null)
                            {
                                empleado = E;
                                empleado.PersonaId = persona.PersonaId;
                                empleado.EsEliminado = NoEsEliminado;
                                empleado.FechaGraba = DateTime.Now;

                                var ListaEmpleados = (from a in ctx.Empleados where a.PersonaId == persona.PersonaId select a).ToList();

                                foreach (var LE in ListaEmpleados)
                                    LE.EsEliminado = EsEliminado;

                                ctx.Empleados.Add(empleado);
                                ctx.SaveChanges();
                            }

                            C.CursoId = Row.GetCell(11) != null ? int.Parse(Row.GetCell(11).ToString()) : 0;

                            if (C.CursoId != 0)
                            {
                                Curso curso = (from a in ListaCursos where a.CursoId == C.CursoId select a).FirstOrDefault();

                                if (curso == null)
                                {
                                    curso = C;
                                    curso.EsEliminado = NoEsEliminado;
                                    curso.FechaGraba = DateTime.Now;

                                    ctx.Cursos.Add(curso);
                                    ctx.SaveChanges();
                                }


                                DateTime fecha = Row.GetCell(12) != null ? Row.GetCell(12).DateCellValue : DateTime.Now;

                                CursoProgramado cursoProgramado = (from a in ctx.CursosProgramados
                                                                   where a.CursoId == curso.CursoId && a.FechaInicio == fecha
                                                                   select a).FirstOrDefault();

                                if(cursoProgramado == null)
                                {
                                    CursoProgramado CP = new CursoProgramado()
                                    {
                                        CursoId = curso.CursoId,
                                        EventoId = 1,
                                        EsEliminado = NoEsEliminado,
                                        FechaInicio = fecha,
                                        FechaFin = fecha,
                                        FechaGraba = DateTime.Now
                                    };

                                    cursoProgramado = CP;

                                    ctx.CursosProgramados.Add(cursoProgramado);
                                    ctx.SaveChanges();
                                }

                                string NombreSalon = Row.GetCell(14) != null ? Row.GetCell(14).ToString() : "";
                                var salon = string.IsNullOrWhiteSpace(NombreSalon) ? ListaSalones.FirstOrDefault() : ListaSalones.Where(x => x.Nombre.ToUpper().Contains(Row.GetCell(14).ToString().ToUpper())).FirstOrDefault();
                                int salonID = 0;

                                if(salon == null)
                                {
                                    EventoSalon ES = new EventoSalon()
                                    {
                                        EsEliminado = NoEsEliminado,
                                        EventoId = 1,
                                        Nombre = NombreSalon,
                                        FechaGraba = DateTime.Now
                                    };

                                    ctx.EventoSalones.Add(ES);
                                    ctx.SaveChanges();
                                    salonID = ES.EventoSalonId;
                                }
                                else
                                {
                                    salonID = salon.Valor;
                                }

                                int? capacitadorId = Row.GetCell(13) != null ? ListaCapacitadores.Where(x => x.Nombre.ToUpper().Contains(Row.GetCell(13).ToString().ToUpper())).FirstOrDefault().Valor : 0;

                                if (capacitadorId == 0)
                                    capacitadorId = null;

                                SalonProgramado salonProgramado = (from a in ctx.SalonProgramados
                                                                   where a.CursoProgramadoId == cursoProgramado.CursoProgramadoId && a.CapacitadorId == capacitadorId
                                                                   select a).FirstOrDefault();

                                if(salonProgramado == null)
                                {
                                    SalonProgramado SP = new SalonProgramado()
                                    {
                                        CursoProgramadoId = cursoProgramado.CursoProgramadoId,
                                        CapacitadorId = capacitadorId,
                                        EventoSalonId = salonID,
                                        NroCupos = 40,
                                        EsEliminado = NoEsEliminado,
                                        FechaGraba = DateTime.Now
                                    };

                                    salonProgramado = SP;

                                    ctx.SalonProgramados.Add(salonProgramado);
                                    ctx.SaveChanges();
                                }

                                SalonClases salonClases = (from a in ctx.SalonClases
                                                           where a.SalonProgramadoId == salonProgramado.SalonProgramadoId
                                                           select a).FirstOrDefault();

                                if(salonClases == null)
                                {
                                    SalonClases SC = new SalonClases()
                                    {
                                        EsEliminado = NoEsEliminado,
                                        SalonProgramadoId = salonProgramado.SalonProgramadoId,
                                         FechaInicio = fecha,
                                         FechaFin = fecha,
                                         FechaGraba = DateTime.Now
                                    };

                                    salonClases = SC;

                                    ctx.SalonClases.Add(salonClases);
                                    ctx.SaveChanges();
                                }

                                decimal nota = 0;

                                if (Row.GetCell(16) != null)
                                    decimal.TryParse(Row.GetCell(16).ToString(),out nota);


                                decimal notaFinal = 0;

                                if (Row.GetCell(20) != null)
                                    decimal.TryParse(Row.GetCell(20).ToString(), out notaFinal);

                                int condicion = Row.GetCell(21) != null ? ListaCondicion.Where(x => x.Nombre.ToUpper().Contains(Row.GetCell(21).ToString().ToUpper())).FirstOrDefault().Valor : 0;

                                EmpleadoCurso empleadoCurso = (from a in ctx.EmpleadoCursos
                                                               where a.EmpleadoId == empleado.EmpleadoId && a.SalonProgramadoId == salonProgramado.SalonProgramadoId
                                                               select a).FirstOrDefault();

                                if(empleadoCurso == null)
                                {
                                    EmpleadoCurso EC = new EmpleadoCurso()
                                    {
                                        EmpleadoId = empleado.EmpleadoId,
                                        SalonProgramadoId = salonProgramado.SalonProgramadoId,
                                        Nota = nota,
                                        CondicionId = condicion,
                                        NotaFinal = notaFinal,
                                        FechaGraba = DateTime.Now,
                                        EmpresaId = empleado.EmpresaId
                                    };

                                    empleadoCurso = EC;

                                    ctx.EmpleadoCursos.Add(empleadoCurso);
                                    ctx.SaveChanges();
                                }


                                int asistio = Row.GetCell(15) == null ? 0 : (int)Math.Round(decimal.Parse(string.IsNullOrWhiteSpace(Row.GetCell(15).ToString()) ? "0" : Row.GetCell(15).ToString()));

                                EmpleadoAsistencia empleadoAsistencia = (from a in ctx.EmpleadoAsistencias
                                                                         where a.EmpleadoCursoId == empleadoCurso.EmpleadoCursoId
                                                                         select a).FirstOrDefault();

                                if(empleadoAsistencia == null)
                                {
                                    EmpleadoAsistencia EA = new EmpleadoAsistencia()
                                    {
                                        EmpleadoCursoId = empleadoCurso.EmpleadoCursoId,
                                        FechaClase = fecha,
                                        EsEliminado = NoEsEliminado,
                                        Asistio = asistio,
                                        FechaGraba = DateTime.Now
                                    };

                                    empleadoAsistencia = EA;

                                    ctx.EmpleadoAsistencias.Add(empleadoAsistencia);
                                    ctx.SaveChanges();
                                }

                                RegistrosInsertados++;
                            }
                        }
                        else
                        {
                            finArchivo = true;
                        }
                    }
                    catch(Exception e)
                    {
                        RegistrosErrados++;
                    }
                }

                return new MultiDataModel() { Int1 = RegistrosInsertados, Int2 = RegistrosErrados};
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
