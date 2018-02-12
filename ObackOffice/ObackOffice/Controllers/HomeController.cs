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
    }
}