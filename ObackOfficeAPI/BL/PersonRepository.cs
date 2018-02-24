using System;
using System.Collections.Generic;
using System.Linq;
using BE.Comun;
using BE.Acceso;
using BE;
using DAL;
using System.Threading;

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

                Thread T = new Thread(new ThreadStart(Utils.SendSimpleMail(body, subject, adresses, CorreoSistema, ClaveCorreo, CorreoHost)));

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
    }
}
