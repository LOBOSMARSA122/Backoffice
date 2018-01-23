using ObackOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObackOffice.Controllers.Person
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index()
        {
            Api API = new Api();
            string url = "Person/GetAll";
            ViewBag.RESPONSE = API.Get<List<Models.Person>>(url);
            return View();
        }
    }
}