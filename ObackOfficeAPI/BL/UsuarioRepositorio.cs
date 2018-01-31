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

        public Usuario LoginUsuario(string usuario, string contrasenia)
        {
            try
            {
                var query = (from a in ctx.Usuarios
                            where a.NombreUsuario == usuario && a.Contrasenia == contrasenia
                             select a).FirstOrDefault();
                return query;
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
