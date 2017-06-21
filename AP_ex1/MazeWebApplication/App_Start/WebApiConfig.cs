using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MazeWebApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi1",
                routeTemplate: "api/{controller}/{name}/{rows}/{cols}"
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "api/{controller}/{name}/{algoId}",
                defaults: new { name = RouteParameter.Optional, algoId = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
