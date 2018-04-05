using BE.Comun;
using BL;
using System;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;

namespace ObackOfficeAPI.Controllers.Reportes
{
    public class ReporteCumplimientoController : ApiController
    {
        private ReporteCumplimientoRepository rcr = new ReporteCumplimientoRepository();
        private ReporteMultipleRepository rmr = new ReporteMultipleRepository();

        [HttpPost]
        public IHttpActionResult BandejaReporteCumplimiento(BandejaReporteCumplimiento data)
        {
            var result = rcr.BandejaReporteCumplimiento(data);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAutocomplete(string campo, string valor)
        {
            List<string> data = new List<string>();
            switch (campo)
            {
                case "area":
                    {
                        data = rmr.GetAreaAutocomplete(valor);
                        break;
                    }
                case "categoria":
                    {
                        data = rmr.GetCategoriaAutocomplete(valor);
                        break;
                    }
                case "empresa":
                    {
                        data = rmr.GetEmpresaAutocomplete(valor);
                        break;
                    }
            }

            return Ok(data);
        }
    }
}
