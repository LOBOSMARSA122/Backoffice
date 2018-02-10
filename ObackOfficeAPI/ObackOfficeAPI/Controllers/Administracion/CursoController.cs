using BE.Comun;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Administracion
{
    public class CursoController : ApiController
    {
        private CursoRepository cr = new CursoRepository();

        [HttpGet]
        public IHttpActionResult ddlCurso()
        {
            List<Dropdownlist> result = cr.ddlCursos();
            return Ok(result);
        }
    }
}
