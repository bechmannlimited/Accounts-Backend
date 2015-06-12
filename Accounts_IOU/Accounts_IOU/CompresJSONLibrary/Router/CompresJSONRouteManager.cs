using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace CompresJSON
{
    public class CompresJSONRouteManager
    {
        public static string SecretUrlPrefix = "routeh";
        public static string WebApiSecretUrlPrefix = "apih";

        //MVC route setup
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Add(
            new Route(
                SecretUrlPrefix + "/{c}/{a}/{id}",
                new RouteValueDictionary(new { controller = "", action = "", id = UrlParameter.Optional }),
                null,
                new DecryptMVCRouteHandler())
            );
        }

        //Web Api Route setup
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "SecretDefaultApi",
                routeTemplate: WebApiSecretUrlPrefix + "/{c}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: new DecryptWebApiRouteHandler(GlobalConfiguration.Configuration)
            );
        }

        public static string EncryptSecretUrlComponent(string str)
        {
            return Encryptor.Encrypt(str);
        }

        public static string DecryptSecretUrlComponent(string str)
        {
            return Encryptor.Decrypt(str);
        }
    }
}