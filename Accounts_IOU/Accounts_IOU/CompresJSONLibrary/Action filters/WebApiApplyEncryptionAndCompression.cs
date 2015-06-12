using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;

namespace CompresJSON
{
    [EncryptAndCompressAsNecessaryWebApi]
    [DecryptAndDecompressAsNecessaryWebApi]
    public class EncryptAndCompressAsNecessaryWebApi : ActionFilterAttribute
    {

        //after
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var data = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

            //string serializedString = (new JavaScriptSerializer()).Serialize(data);
            //serializedString = serializedString.Replace("\\\"", "#").Replace("\"", "");
            string encryptedString = CompresJSON.EncryptAndCompressAsNecessary(data);

            var rc = new Dictionary<string, object>();
            rc["data"] = encryptedString;

            actionExecutedContext.Response.Content = new StringContent((new JavaScriptSerializer()).Serialize(rc));

            base.OnActionExecuted(actionExecutedContext);
        }


    }

    public class DecryptAndDecompressAsNecessaryWebApi : ActionFilterAttribute
    {

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            NameValueCollection postedParams = HttpContext.Current.Request.Params;
            Dictionary<string, string> httpBodyDictionary = new Dictionary<string, string>();

            foreach (var key in postedParams.AllKeys)
            {
                httpBodyDictionary[key] = postedParams[key].ToString();
            }

            if (httpBodyDictionary.ContainsKey("data") && httpBodyDictionary["data"] != null)
            {
                //assume encrypted + compressed for now
                var d = httpBodyDictionary["data"];

                string json = CompresJSON.DecryptAndDecompressAsNecessary(d) ;//.Replace("#", "\"");
                var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(json);

                foreach (var key in dict.Keys)
                {
                    actionContext.ActionArguments[key] = dict[key];
                    actionContext.ControllerContext.RouteData.Values[key] = dict[key];
                }

                var mvcActionModelParameters = actionContext.ActionDescriptor.GetParameters();

                foreach (var parameter in mvcActionModelParameters)
                {
                    string typeName = parameter.ParameterType.FullName; // "System.String";

                    object o = null;

                    if (!typeName.Contains("System"))
                    {
                        o = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(typeName);
                        o = Mapper.ToObject(actionContext.ControllerContext.RouteData.Values, o);
                    }

                    if (o != null)
                    {
                        actionContext.ActionArguments[parameter.ParameterName] = o;
                        actionContext.ControllerContext.RouteData.Values[parameter.ParameterName] = o;
                    }
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

    }
}