using BE.Comun;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;

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

        [HttpPost]
        public IHttpActionResult ReporteAcademicoExcel(BandejaReporteAcademico data)
        {
            string fullPath = HostingEnvironment.MapPath(@"~/Plantillas Excel/Plantilla Reporte Academico.xlsx");
            FileStream TemplateFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            MemoryStream response = rar.ReporteAcademicoExcel(data,TemplateFile);

            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult BandejaReporteMultiple(BandejaReporteMultiple data)
        {
            BandejaReporteMultiple response = rar.BandejaReporteMultiple(data);
            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult Chart(MultiDataModel data)
        {
            string Action = data.String1;
            List<ReporteMultipleList> Lista = JsonConvert.DeserializeObject<List<ReporteMultipleList>>(data.String2);

            switch (Action)
            {
                case "Asistencia":
                    {
                        return Ok(Convert.ToBase64String(rar.ChartAsistencia(Lista)));
                    }
                case "Aprobados":
                    {
                        return Ok(Convert.ToBase64String(rar.ChartAprobados(Lista)));
                    }
                case "Promedio":
                    {
                        return Ok(Convert.ToBase64String(rar.ChartPromedio(Lista)));
                    }
            }

            return BadRequest("No se encontró la acción dentro del controlador.");
        }

        [HttpPost]
        public IHttpActionResult BandejaReporteMultipleExcel(MultiDataModel multi)
        {
            string fullPath = HostingEnvironment.MapPath(@"~/Plantillas Excel/Plantilla Reporte Academico.xlsx");
            FileStream TemplateFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            BandejaReporteMultiple data = JsonConvert.DeserializeObject<BandejaReporteMultiple>(multi.String1);

            MemoryStream response = rar.BandejaReporteMultipleExcel(data, TemplateFile);

            return Ok(response);
        }
    }
}
