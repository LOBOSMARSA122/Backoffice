using BE.Cliente;
using BL;
using BE.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Comun
{
    public class EmpleadoController : ApiController
    {
        private EmpleadoRepository er = new EmpleadoRepository();

        [HttpGet]
        public IHttpActionResult GetEmpleados(string valor)
        {
            List<string> result = er.GetEmpleadosString(valor);
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

        [HttpGet]
        public IHttpActionResult VerificaYRegistraEmpleado(string usuario, string email, string cargo, string pass, string telefono)
        {
            string response = er.VerificaYRegistraEmpleado(usuario, email, cargo, pass, telefono);
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult ObtenerHistorialEmpleado(int usuarioId)
        {
            if (usuarioId == 0)
                return BadRequest("Parámetro incorrecto");

            List<ReporteMultipleList> data = er.ObtenerHistorialEmpleado(usuarioId);
            return Ok(data);
        }

        [HttpPost]
        public IHttpActionResult DownloadFile(data data)
        {
            string directorioExamenes = string.Format("{0}{1}\\", System.Web.Hosting.HostingEnvironment.MapPath("~/"), System.Configuration.ConfigurationManager.AppSettings["directorioExamenes"].ToString());
            string directorioDiplomas = string.Format("{0}{1}\\", System.Web.Hosting.HostingEnvironment.MapPath("~/"), System.Configuration.ConfigurationManager.AppSettings["directorioDiplomas"].ToString());

            MemoryStream response = er.DownloadFile(data.documento, directorioExamenes, directorioDiplomas);

            return Ok(response);
        }
    }
}
