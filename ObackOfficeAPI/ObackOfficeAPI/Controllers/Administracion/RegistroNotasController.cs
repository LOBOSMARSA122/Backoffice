﻿using BE.Administracion;
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
    public class RegistroNotasController : ApiController
    {
        private RegistroNotasRepository rr = new RegistroNotasRepository();

        [HttpPost]
        public IHttpActionResult GetRegistroNotas(BandejaRegistroNotas data)
        {
            BandejaRegistroNotas result = rr.BandejaRegistroNotas(data);
            return Ok(result);
        }
    }
}