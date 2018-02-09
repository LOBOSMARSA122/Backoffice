using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Comun
{
    public class PersonaController : ApiController
    {
        private PersonaRepository pr = new PersonaRepository();

        [HttpGet]
        public IHttpActionResult getFoto(int personaId)
        {
            byte[] result = pr.getFoto(personaId);
            return Ok(result);
        }
    }
}
