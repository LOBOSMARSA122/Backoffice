using ObackOffice.Controllers.Seguridad;
using ObackOffice.Models;
using ObackOffice.Models.Comun;
using ObackOffice.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Reportes
{
    public class ReporteCumplimientoController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            //Api API = new Api();
            //Dictionary<string, string> args = new Dictionary<string, string>
            //{
            //    { "Empresa", ""}
            //};
            //ViewBag.REGISTROS = API.Get<BandejaReporteCumplimiento>("ReporteCumplimiento/BandejaReporteCumplimiento", args);
            ViewBag.REGISTROS = new BandejaReporteCumplimiento() { Lista = new List<ReporteCumplimientoList>(), Take = 10 };

            return View();
        }

        public ActionResult FiltrarReporteCumplimiento(string empresa)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Empresa", empresa}              
            };
            ViewBag.REGISTROS = API.Post<BandejaReporteCumplimiento>("ReporteCumplimiento/BandejaReporteCumplimiento", args);
            return PartialView("_ReporteCumplimientoPartial");
        }


        public JsonResult Autocomplete(string campo, string valor)
        {
            Api API = new Api();
            string url = "ReporteMultiple/GetAutocomplete";
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "campo",campo},
                { "valor",valor }
            };
            List<string> data = API.Get<List<string>>(url, args);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}