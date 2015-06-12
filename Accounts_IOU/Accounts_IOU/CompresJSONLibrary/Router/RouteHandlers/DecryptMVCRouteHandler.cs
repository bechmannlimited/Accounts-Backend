using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CompresJSON
{
    public class DecryptMVCRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var routeValues = requestContext.RouteData.Values;

            routeValues["Controller"] = CompresJSONRouteManager.DecryptSecretUrlComponent(routeValues["c"].ToString());
            routeValues["Action"] = CompresJSONRouteManager.DecryptSecretUrlComponent(routeValues["a"].ToString());

            if (routeValues.ContainsKey("id"))
            {
                string id = CompresJSONRouteManager.DecryptSecretUrlComponent(routeValues["id"].ToString());
                routeValues["id"] = id;
            }

            routeValues["c"] = null;
            routeValues["a"] = null;

            var mvcRouteHandler = new MvcRouteHandler();
            return (mvcRouteHandler as IRouteHandler).GetHttpHandler(requestContext);
        }
    }
}