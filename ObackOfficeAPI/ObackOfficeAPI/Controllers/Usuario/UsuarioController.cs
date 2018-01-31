using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Acceso;

namespace ObackOfficeAPI.Controllers.Usuario
{
    public class UsuarioController : ApiController
    {
        private UsuarioRepositorio ur = new UsuarioRepositorio();

        [HttpGet]
        public IHttpActionResult GetUsuario(string usuario, string contrasenia)
        {
            BE.Acceso.UsuarioAutorizado result = ur.LoginUsuario(usuario, contrasenia);
            return Ok(result);
          
            
        }

        [HttpGet]
        public IHttpActionResult GetAutorizacion(int rolId)
        {
            List<Autorizacion> result = ur.GetAutorizacion(rolId);
            return Ok(result);
        }
    }
}
