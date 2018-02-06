﻿using System;
using BL;
using BE.Administracion;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Administracion
{
    public class EmpresasController : ApiController
    {
        private EmpresasRepositorio er = new EmpresasRepositorio();

        [HttpGet]
        public IHttpActionResult GetEmpresas()
        {
            List<Empresa> result = er.GetEmpresas();
            return Ok(result);
        }
    }
}