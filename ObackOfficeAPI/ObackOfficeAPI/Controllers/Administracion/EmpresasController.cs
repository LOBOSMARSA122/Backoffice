using System;
using BL;
using BE.Comun;
using BE.Administracion;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Comun;

namespace ObackOfficeAPI.Controllers.Administracion
{
    public class EmpresasController : ApiController
    {
        private EmpresasRepositorio er = new EmpresasRepositorio();

        [HttpGet]
        public IHttpActionResult GetEmpresas()
        {
            List<Dropdownlist> result = er.GetEmpresas();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ddlEmpresas()
        {
            List<Dropdownlist> result = er.ddlEmpresa();
            return Json(result);
        }
    }
}
