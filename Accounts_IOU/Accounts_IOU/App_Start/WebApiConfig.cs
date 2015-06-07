using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Accounts_IOU
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "FriendsApi",
                routeTemplate: "api/Users/{userID}/Friends/{id}",
                defaults: new { controller = "Friends", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "UserTransactionsApi",
                routeTemplate: "api/Users/{userID}/Transactions/{id}",
                defaults: new { controller = "Transactions", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
