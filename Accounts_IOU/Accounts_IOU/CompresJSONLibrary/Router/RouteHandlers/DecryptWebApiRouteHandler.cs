using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;

namespace CompresJSON
{
    class DecryptWebApiRouteHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var routeValues = request.GetRequestContext().RouteData.Values;

            string c = CompresJSONRouteManager.DecryptSecretUrlComponent(routeValues["c"].ToString());
            routeValues["c"] = null;
            routeValues["Controller"] = c;

            if (routeValues.ContainsKey("id"))
            {
                string id = CompresJSONRouteManager.DecryptSecretUrlComponent(routeValues["id"].ToString());
                routeValues["id"] = null;
                routeValues["id"] = id;
            }

            return base.SendAsync(request, cancellationToken);
        }

        public DecryptWebApiRouteHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration); 
        }
    }
}