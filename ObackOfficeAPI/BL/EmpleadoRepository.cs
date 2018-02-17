﻿using BE.Cliente;
using BE.Comun;
using BE.Acceso;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Contrasenia = pass,
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

                return "Ok";
            }
            catch(Exception e)
            {
                return "Sucedió un problema al intentar registrar.";
            }
        }
    }
}
