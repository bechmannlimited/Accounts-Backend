using Accounts_IOUression;
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
            bool shouldEncryptManually = Tools.GetValueFromRequestHeader(actionExecutedContext.Request.Headers, "CompresJSON-Encrypt") == "true";
            bool shouldCompressManually = Tools.GetValueFromRequestHeader(actionExecutedContext.Request.Headers, "CompresJSON-Compress") == "true";

            if (shouldEncryptManually)
            {
                var data = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

                string encryptedString = CompresJSON.EncryptAndCompress(data, shouldEncryptManually, shouldCompressManually);

                var rc = new Dictionary<string, object>();
                rc["data"] = encryptedString;

                actionExecutedContext.Response.Content = new StringContent((new JavaScriptSerializer()).Serialize(rc));
            }

            string acceptedEncoding = Tools.GetValueFromRequestHeader(actionExecutedContext.Request.Headers, "Accept-Encoding1");

            if (acceptedEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase) || acceptedEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
            {
                if (actionExecutedContext.Response.Content != null) 
                {
                    actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, acceptedEncoding);
                }
                //actionExecutedContext.Response.Headers.Add("Content-Encodingasdf", acceptedEncoding);
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }

    public class DecryptAndDecompressAsNecessaryWebApi : ActionFilterAttribute
    {

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            bool shouldEncryptManually = Tools.GetValueFromRequestHeader(actionContext.Request.Headers, "CompresJSON-Encrypt") == "true";
            bool shouldCompressManually = Tools.GetValueFromRequestHeader(actionContext.Request.Headers, "CompresJSON-Compress") == "true";

            if (shouldEncryptManually)
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

                    string json = CompresJSON.DecryptAndDecompress(d, shouldEncryptManually, shouldCompressManually); //.Replace("#", "\"");
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
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

    }
}