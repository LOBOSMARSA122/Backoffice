﻿using BE.Administracion;
using BE.Cliente;
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
    public class CursoProgramadoController : ApiController
    {
        private CursoProgramadoRepository cpr = new CursoProgramadoRepository();

        [HttpGet]
        public IHttpActionResult CursosProgramados(int cursoId)
        {
            List<Agenda> result = cpr.CursosProgramados(cursoId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetEmpleadosCurso(int salonProgramadoId)
        {
            List<EmpleadoInscrito> result = cpr.GetEmpleadosCurso(salonProgramadoId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ddlCursoProgramdos(int eventoId)
        {
            List<Dropdownlist> result = cpr.ddlCursoProgramdos(eventoId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ddlSalonProgramado(int cursoProgramadoId)
        {
            List<Dropdownlist> result = cpr.ddlSalonProgramado(cursoProgramadoId);
            return Ok(result);
        }

    }
}
