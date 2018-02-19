using ObackOffice.Utils;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using ObackOffice.Models.Acceso;

namespace ObackOffice.Controllers.Seguridad
{
    public class GeneralSecurityAttribute : ActionFilterAttribute
    {
        public string Rol { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ClientSession Usuario = (ClientSession)filterContext.HttpContext.Session.Contents["AutBackoffice"];
            if(Usuario == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                                       { "action", "Login" },
                                       { "controller", "Generals" }});
                return;
            }

            filterContext.Controller.ViewBag.USUARIO = Usuario;


            if (string.IsNullOrWhiteSpace(Rol))
                return;


            bool aceptado = false;
            Autorizacion AUT =  Usuario.Autorizacion.Where(x => x.Descripcion.Contains(Rol.Split('-')[0])).FirstOrDefault();
            if (AUT != null) {
                if (AUT.SubMenus.Where(x => x.Descripcion.Contains(Rol.Split('-')[1])).FirstOrDefault() != null)
                    aceptado = true;
            }

            if (!aceptado)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                                       { "action", "Home" },
                                       { "controller", "Generals" }});
                return;
            }


            return;
        }
    }
}