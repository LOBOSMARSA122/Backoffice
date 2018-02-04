using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Acceso;
using BE.Comun;

namespace ObackOfficeAPI.Controllers.Usuario
{
    public class UsuarioController : ApiController
    {
        private UsuarioRepositorio ur = new UsuarioRepositorio();

        [HttpGet]
        public IHttpActionResult GetUsuario(string usuario, string contrasenia)
        {
            UsuarioAutorizado result = ur.LoginUsuario(usuario, contrasenia);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAutorizacion(int rolId)
        {
            if (rolId == 0)
                return BadRequest("Rol Inválido");

            List<Autorizacion> result = ur.GetAutorizacion(rolId);
            return Ok(result);
        }
    }
}
