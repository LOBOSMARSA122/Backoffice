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
    public class ReporteMultipleController : ApiController
    {
        private ReporteMultipleRepository rmr = new ReporteMultipleRepository();

        [HttpPost]
        public IHttpActionResult BandejaReporteMultiple(BandejaReporteMultiple data)
        {
            BandejaReporteMultiple response = rmr.BandejaReporteMultiple(data);
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
                        return Ok(Convert.ToBase64String(rmr.ChartAsistencia(Lista)));
                    }
                case "Aprobados":
                    {
                        return Ok(Convert.ToBase64String(rmr.ChartAprobados(Lista)));
                    }
                case "Promedios":
                    {
                        return Ok(Convert.ToBase64String(rmr.ChartPromedio(Lista)));
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

            MemoryStream response = rmr.BandejaReporteMultipleExcel(data, TemplateFile);

            return Ok(response);
        }
    }
}
