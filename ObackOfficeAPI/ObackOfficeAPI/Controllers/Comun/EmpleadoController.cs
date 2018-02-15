using BE.Cliente;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Comun
{
    public class EmpleadoController : ApiController
    {
        private EmpleadoRepository er = new EmpleadoRepository();

        [HttpGet]
        public IHttpActionResult GetEmpleados(string valor, int empresaId)
        {
            List<string> result = er.GetEmpleadosString(valor, empresaId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult DatosEmppleado(string nroDocumento)
        {
            Empleado result = er.GetEmpleadoByDocumento(nroDocumento);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult GrabarEmpleado(dataEmpleado data)
        {
            bool result = er.GrabarEmpleado(data);
            return Ok(result);
        }
    }
}
