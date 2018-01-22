using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Web
{
    public class ObackOfficePruebaController : ApiController
    {
        [HttpGet]
        public string PruebaGet()
        {
            return "ObackOffice Prueba Get";
        }

        [HttpPost]
        public string PruebaPost()
        {
            return "ObackOffice Prueba Post";
        }

        [HttpPut]
        public string PruebaPut()
        {
            return "ObackOffice Prueba Put";
        }
    }
}
