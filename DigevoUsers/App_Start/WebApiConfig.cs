using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DigevoUsers {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services

            // WARNING: Enable CORS --TEMPORALY-- for consuming
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*config.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "api/service/RequestCustodyX",
                defaults: new { controller = "Service", action = "RequestCustodyX" }
            );*/
        }
    }
}
