using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiWalkthrough2.Infrastructure;

namespace WebApiWalkthrough2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                "default",
                "api2/{controller}");

            config.Filters.Add(new TestAuthenticationFilterAttribute());
            //config.Filters.Add(new TestAuthorizationFilterAttribute());
        }
    }
}
