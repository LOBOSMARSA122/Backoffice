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

        [HttpPost]
        public IHttpActionResult GetUsuarios(BandejaUsuario data)
        {
            BandejaUsuario result = ur.GetUsuarios(data);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetUsuario(int id)
        {
            BE.Acceso.Usuario result = ur.GetUsuario(id);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult DeleteUser(MultiDataModel data)
        {
            if (data.Int1 == 0)
                return BadRequest("Información Inválida");

            if (data.Int2 == 0)
                return BadRequest("Sesión Expirada");

            bool response = ur.DeleteUser(data.Int1, data.Int2);
            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult BandejaUsuarioExcel(BandejaUsuario data)
        {
            System.IO.MemoryStream response = ur.BandejaUsuarioExcel(data);

            return Ok(response);
        }
    }
}
