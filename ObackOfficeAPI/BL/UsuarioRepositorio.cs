using System;
using System.Collections.Generic;
using System.Linq;
using BE.Acceso;
using DAL;

namespace BL
{
    public class UsuarioRepositorio
    {
        private DatabaseContext ctx = new DatabaseContext();

        public UsuarioAutorizado LoginUsuario(string usuario, string contrasenia)
        {
            try
            {
                UsuarioAutorizado oUsuarioAutorizado = new UsuarioAutorizado();
                var qUsuario = (from a in ctx.Usuarios
                                join b in ctx.Personas on a.PersonaId equals b.PersonaId
                                join c in ctx.Parametros on new {a = a.RolId, b=102} equals new {a = c.ParametroId, b= c.GrupoId}
                                where a.NombreUsuario == usuario && a.Contrasenia == contrasenia
                                select new UsuarioAutorizado {
                                    UsuarioId = a.UsuarioId,
                                    PersonaId = a.PersonaId,
                                    NombreUsuario = a.NombreUsuario,
                                    NombreCompleto = b.Nombres + " " + b.ApellidoPaterno + " " + b.ApellidoMaterno,
                                    FechaCaduca = a.FechaCaduca,
                                    RolId = a.RolId,
                                    Rol = c.Valor1

                             }).FirstOrDefault();
                if (qUsuario != null)
                {
                    var qAutu = GetAutorizacion(qUsuario.RolId);
                    qUsuario.Autorizacion = qAutu;
                }
                else
                {
                    return null;
                }               

                return qUsuario;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Autorizacion> GetAutorizacion(int rolId)
        {

            var perfil = (from a in ctx.Perfiles
                          join b in ctx.Menus on a.MenuId equals b.MenuId
                          where a.RolId == rolId && a.EsEliminado == 0
                          select new SubMenu
                          {
                              MenuId = a.MenuId,
                              Descripcion = b.Descripcion,
                              PadreId = b.PadreId
                          }).ToList();

            var query = (from a in ctx.Perfiles
                        join b in ctx.Menus on a.MenuId equals b.MenuId
                         where a.RolId == rolId && a.EsEliminado == 0 && b.PadreId ==-1
                         select new Autorizacion
                         {
                             PerfilId = a.PerfilId,
                             RolId = a.RolId,
                             MenuId = b.MenuId,
                             Descripcion = b.Descripcion,
                             PadreId = b.PadreId,
                             Icono = b.Icono
                         }).ToList();
            query.ForEach(a =>
            {
                a.SubMenus = perfil.FindAll(p => p.PadreId == a.MenuId);
            });

            return query;


        }
    }
}
