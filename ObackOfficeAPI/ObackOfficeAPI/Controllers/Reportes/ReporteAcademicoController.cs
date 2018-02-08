﻿using BE.Comun;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Reportes
{
    public class ReporteAcademicoController : ApiController
    {
        private ReporteAcademicoRepository rar = new ReporteAcademicoRepository();

        [HttpPost]
        public IHttpActionResult BandejaReporteAcademico(BandejaReporteAcademico data)
        {
            data = rar.BandejaReporteAcademico(data);
            return Ok(data);
        }

        [HttpGet]
        public IHttpActionResult DetalleEmpleado(int PersonaId, int cursoProgramadoId)
        {
            return Ok(rar.DetalleEmpleado(PersonaId, cursoProgramadoId));
        }

        [HttpGet]
        public IHttpActionResult TallerEmpleado(int PersonaId, int cursoProgramadoId)
        {
            return Ok(rar.TallerEmpleado(PersonaId,cursoProgramadoId));
        }
    }
}
