using BE.Comun;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Comun
{
    public class ParametroController : ApiController
    {
        private ParametroRepository pr = new ParametroRepository();
        [HttpGet]
        public IHttpActionResult GetParametroByGrupoId(int grupoId, string accion)
        {
            List<Dropdownlist> result = pr.GetParametroByGrupoId(grupoId, accion);
            return Ok(result);
        }
    }
}
