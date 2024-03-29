﻿using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace Shodypati
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                "ActionApi",
                "api/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
            );


            config.Routes.MapHttpRoute(
                "ActionApi2",
                "api/{controller}/{action}/{categoryid}/{merchantid}",
                new {categoryid = RouteParameter.Optional, merchantid = RouteParameter.Optional}
            );


            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );
        }
    }
}