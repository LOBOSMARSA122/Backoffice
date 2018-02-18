﻿using BE.Cliente;
using BE.Comun;
using BE.Acceso;
using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
   public class EmpleadoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<BusquedaEmpleado> GetEmpleados (string valor, int empresaId)
        {
            try
            {
                var query = (from a in ctx.Personas
                             join b in ctx.Empleados on a.PersonaId equals b.PersonaId
                             where a.EsEliminado == 0 && b.EmpresaId == empresaId
                             && (a.Nombres.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.NroDocumento.Contains(valor))
                             select new BusquedaEmpleado
                             {
                                 EmpleadoId = b.EmpleadoId,
                                 PersonaId = b.PersonaId,
                                 EmpresaId = b.EmpresaId,
                                 NombreCompleto = a.Nombres + " " + a.ApellidoPaterno + " " + a.ApellidoMaterno,
                                 NroDocumento = a.NroDocumento
                             }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetEmpleadosString(string valor, int empresaId)
        {
            try
            {
                var query = (from a in ctx.Personas
                             join b in ctx.Empleados on a.PersonaId equals b.PersonaId
                             where a.EsEliminado == 0 && b.EmpresaId == empresaId
                             && (a.Nombres.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.NroDocumento.Contains(valor))
                             select new 
                             {
                                 NombreCompleto = a.Nombres + " " + a.ApellidoPaterno + " " + a.ApellidoMaterno + "*" + a.NroDocumento,
                             
                             }).ToList();

                return query.Select(x => x.NombreCompleto).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Empleado GetEmpleadoByDocumento(string nroDocumento)
        {
            try
            {
                var query = (from a in ctx.Empleados
                             join b in ctx.Personas on a.PersonaId equals b.PersonaId
                             where b.NroDocumento == nroDocumento
                             select a).FirstOrDefault();

                return query;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Boolean GrabarEmpleado(dataEmpleado data)
        {
            try
            {
                //Verificar si el empleado existe en la empresa por TipoDocumento y NroDocumento

                var result = (from a in ctx.Empleados
                              join b in ctx.Personas on a.PersonaId equals b.PersonaId
                              where b.TipoDocumentoId == data.TipoDocumentoId && b.NroDocumento == data.NroDocumento && a.EmpresaId == data.EmpresaId
                              select a).ToList();

                if (result.Count() > 0)
                {
                    return false;
                }

                //Validar si la persona ya se encuetra registrada en el sistema
                var validaPersona = (from a in ctx.Personas where a.NroDocumento == data.NroDocumento && a.TipoDocumentoId == data.TipoDocumentoId select a).ToList();

                if (validaPersona.Count() == 0)
                {
                    Persona oPersona = new Persona();
                    oPersona.Nombres = data.Nombres;
                    oPersona.ApellidoPaterno = data.ApePaterno;
                    oPersona.ApellidoMaterno = data.ApeMaterno;
                    oPersona.TipoDocumentoId = data.TipoDocumentoId;
                    oPersona.NroDocumento = data.NroDocumento;
                    oPersona.GeneroId = -1;
                    oPersona.EsEliminado = 0;
                    oPersona.UsuGraba = data.UsuGraba;
                    ctx.Personas.Add(oPersona);
                    ctx.SaveChanges();
                    int personaId = oPersona.PersonaId;

                    Empleado oEmpleado = new Empleado();
                    oEmpleado.PersonaId = personaId;
                    oEmpleado.EmpresaId = data.EmpresaId;
                    oEmpleado.Cargo = data.Cargo;
                    oEmpleado.Area = data.Area;
                    oEmpleado.EsEliminado = 0;
                    oEmpleado.UsuGraba = data.UsuGraba;
                    oEmpleado.FechaGraba = DateTime.Now;

                    ctx.Empleados.Add(oEmpleado);
                    int rows = ctx.SaveChanges();
                    if (rows > 0)
                        return true;
                    else return false;
                }
                else
                {
                    Empleado oEmpleado = new Empleado();
                    oEmpleado.PersonaId = validaPersona[0].PersonaId;
                    oEmpleado.EmpresaId = data.EmpresaId;
                    oEmpleado.Cargo = data.Cargo;
                    oEmpleado.Area = data.Area;
                    oEmpleado.EsEliminado = 0;
                    oEmpleado.UsuGraba = data.UsuGraba;
                    oEmpleado.FechaGraba = DateTime.Now;

                    ctx.Empleados.Add(oEmpleado);
                    int rows = ctx.SaveChanges();
                    if (rows > 0)
                        return true;
                    else return false;
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string VerificaYRegistraEmpleado(string usuario, string email, string cargo, string pass, string telefono)
        {
            try
            {
                int NoEliminado = (int)Enumeradores.EsEliminado.No;
                int RolEmpleado = (int)Enumeradores.Roles.Empleado;

                var Persona = (from a in ctx.Personas where a.NroDocumento == usuario select a).FirstOrDefault();

                if (Persona == null)
                    return "El DNI ingresado no es válido";

                var Empleados = (from a in ctx.Empleados where a.PersonaId == Persona.PersonaId select a).ToList();

                if (Empleados.Count == 0 || Empleados.Where(x => x.EsEliminado == NoEliminado).ToList().Count == 0)
                    return "Usted no se encuentra activo en ninguna empresa";

                if (Empleados.Where(x => x.EsEliminado == NoEliminado).ToList().Count > 1)
                    return "Usted se encuentra activo en más de una empresas, no puede continuar con el registro";

                var Empleado = Empleados.Where(x => x.EsEliminado == NoEliminado).FirstOrDefault();

                var Usuario = (from a in ctx.Usuarios where a.PersonaId == Persona.PersonaId select a).FirstOrDefault();

                if (Usuario != null)
                    return "Usted ya posee una cuenta";


                Usuario NewUser = new Usuario()
                {
                    Contrasenia = Utils.Encrypt(pass),
                    EmpresaId = Empleado.EmpresaId,
                    EsEliminado = NoEliminado,
                    FechaGraba = DateTime.Now,
                    NombreUsuario = usuario,
                    PersonaId = Persona.PersonaId,
                    RolId = RolEmpleado
                };

                ctx.Usuarios.Add(NewUser);
                ctx.SaveChanges();

                NewUser.UsuGraba = NewUser.UsuarioId;


                if(Persona.CorreoElectronico != email)
                {
                    Persona.CorreoElectronico = email;
                    Persona.UsuActualiza = NewUser.UsuarioId;
                    Persona.FechaActualiza = DateTime.Now;
                }

                if (!string.IsNullOrWhiteSpace(telefono) && (Persona.NumeroCelular != telefono))
                {
                    Persona.NumeroCelular = telefono;
                    Persona.UsuActualiza = NewUser.UsuarioId;
                    Persona.FechaActualiza = DateTime.Now;
                }

                if (Empleado.Cargo != cargo)
                {
                    Empleado.Cargo = cargo;
                    Empleado.UsuActualiza = NewUser.UsuarioId;
                    Empleado.FechaActualiza = DateTime.Now;
                }

                ctx.SaveChanges();

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

                body = body.Replace("[@NOMBRE_PERSONA@]", string.Format("{0} {1} {2}",Persona.Nombres, Persona.ApellidoPaterno, Persona.ApellidoMaterno)).Replace("[@NOMBRE_USUARIO@]", Usuario.NombreUsuario).Replace("[@PASSWORD@]", pass);

                string subject = "Registro Exitoso de Usuario";
                List<string> adresses = new List<string>();
                adresses.Add(Persona.CorreoElectronico);

                Utils.SendSimpleMail(body,subject,adresses, CorreoSistema, ClaveCorreo, CorreoHost);
                
                return "Ok";
            }
            catch(Exception e)
            {
                return "Sucedió un problema al intentar registrar.";
            }
        }

        public List<ReporteMultipleList> ObtenerHistorialEmpleado(int UsuarioId)
        {
            try
            {
                int GrupoCondicion = (int)Enumeradores.GrupoParametros.Condición;
                int NoEliminado = (int)Enumeradores.EsEliminado.No;

                var return_data = (from a in ctx.Empleados
                                   join b in ctx.Personas on a.PersonaId equals b.PersonaId
                                   join c in ctx.Usuarios on b.PersonaId equals c.PersonaId
                                   join d in ctx.EmpleadoCursos on a.EmpleadoId equals d.EmpleadoId
                                   join e in ctx.SalonProgramados on d.SalonProgramadoId equals e.SalonProgramadoId
                                   join f in ctx.CursosProgramados on e.CursoProgramadoId equals f.CursoProgramadoId
                                   join g in ctx.Cursos on f.CursoId equals g.CursoId
                                   join h in ctx.Parametros on new {a = GrupoCondicion, b = d.CondicionId} equals new {a = h.GrupoId, b = h.ParametroId}
                                   join i in ctx.Capacitadores on e.CapacitadorId equals i.CapacitadorId
                                   join j in ctx.Personas on i.PersonaId equals j.PersonaId
                                   where 
                                   a.EsEliminado == NoEliminado &&
                                   b.EsEliminado == NoEliminado &&
                                   c.EsEliminado == NoEliminado &&
                                   d.EsEliminado == NoEliminado &&
                                   g.EsEliminado == NoEliminado &&
                                   c.UsuarioId == UsuarioId
                                   select new ReporteMultipleList()
                                   {
                                       Curso = g.NombreCurso,
                                       InicioCurso = f.FechaInicio,
                                       FinCurso = f.FechaFin,
                                       Nota = d.Nota,
                                       NotaTaller = d.NotaTaller,
                                       Condicion = h.Valor1,
                                       Capacitador = j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno
                                   }).ToList();

                return return_data;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
