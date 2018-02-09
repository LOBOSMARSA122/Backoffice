using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObackOffice.Models;
using ObackOffice.Utils;
using Newtonsoft.Json;
using ObackOffice.Models.Comun;

namespace ObackOffice.Controllers
{
    public class HomeController : Controller
    {
        //ACCION PARA INGRESAR EN /Home/Pagina1
        public ActionResult Pagina1()
        {
            //Instancia de la clase API
            Api API = new Api();

            //URI del servicio a consultar
            string url = "ObackOfficePrueba/PruebaGet";

            //Se hace un llamado al api (GET, POST, PUT) con el tipo de objeto a recibir
            //en este ejemplo se recibe un "string" y los parámetros se envian en una
            //variable diccionario <string,string> (clave, valor)
            ViewBag.RESPONSE = API.Get<string>(url, new Dictionary<string, string>());

            return View("index");
        }

        public ActionResult Pagina2()
        {
            Api API = new Api();
            string url = "ObackOfficePrueba/PruebaPost";

            ViewBag.RESPONSE = API.Post<string>(url, new Dictionary<string, string>());

            return View("index");
        }

        public ActionResult Pagina3()
        {
            Api API = new Api();
            string url = "ObackOfficePrueba/PruebaPut";

            ViewBag.RESPONSE = API.PUT<string>(url, new Dictionary<string, string>());

            return View("index");
        }

        public ActionResult Chart()
        {
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            return View();
        }

        public ActionResult Index()
        {
            Api API = new Api();
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "grupoId", ((int)Enums.Parametros.Sedes).ToString() }
            };
            ViewBag.SEDES = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("Parametro/GetParametroByGrupoId", args), Constantes.All);
            return View();
        }

        public ActionResult FiltrarReporteMultiple(string SedeId, string EventoId, string CursoId, string NombreEmpleado, string DNIEmpleado, string Index, string Take)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "SedeId", SedeId },
                { "EventoId", EventoId },
                { "CursoId", CursoId },
                { "NombreEmpleado", NombreEmpleado },
                { "DNIEmpleado", DNIEmpleado },
                { "Index", Index },
                { "Take", Take }
            };
            ViewBag.REGISTROS = API.Post<BandejaReporteMultiple>("ReporteAcademico/BandejaReporteMultiple", args);
            return PartialView("_ReporteMultiplePartial");
        }
    }
}