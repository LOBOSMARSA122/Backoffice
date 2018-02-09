using ObackOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObackOffice.Helpers;

namespace ObackOffice.Controllers.Comun
{
    public class PersonaController : Controller
    {
        public Helpers.FileResult getFoto(string personaId)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                {"personaID", personaId }
            };

            byte[] foto = API.Get<byte[]>("Persona/getFoto", args);

            if (foto == null)
            {
                string fullPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/static/images/foto@default.png");
                return new Helpers.FileResult(fullPath, "image/png");
            }
            else
            {
                return new Helpers.FileResult(foto, "image/png");
            }
        }
    }
}