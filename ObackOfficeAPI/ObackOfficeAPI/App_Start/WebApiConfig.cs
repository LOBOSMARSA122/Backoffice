using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ObackOfficeAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ObackOfficePrueba",
                routeTemplate: "{controller}/{Action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
