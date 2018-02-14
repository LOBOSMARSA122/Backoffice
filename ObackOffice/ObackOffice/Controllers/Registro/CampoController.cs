using ObackOffice.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Registro
{
    public class CampoController : Controller
    {
        // GET: Campo
        public ActionResult Index()
        {
            ViewBag.USUARIO = ((ClientSession)Session["AutBackoffice"]);
            return View();
        }
    }
}