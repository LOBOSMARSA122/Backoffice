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
    public class CapacitadorController : ApiController
    {
        private CapacitadorRepository cr = new CapacitadorRepository();

        [HttpGet]
        public IHttpActionResult ddlCapacitador()
        {
            List<Dropdownlist> result = cr.ddlCapacitadores();
            return Ok(result);
        }
    }
}
