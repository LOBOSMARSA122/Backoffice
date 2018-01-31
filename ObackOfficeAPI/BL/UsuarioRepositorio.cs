using System;
using System.Collections.Generic;
using System.Linq;
using BE;
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
    }
}
