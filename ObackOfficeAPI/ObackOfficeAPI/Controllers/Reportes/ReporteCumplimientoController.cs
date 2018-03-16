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

        [HttpPost]
        public IHttpActionResult BandejaReporteCumplimiento(BandejaReporteCumplimiento data)
        {
            var result = rcr.BandejaReporteCumplimiento(data);
            return Ok(result);
        }
    }
}
