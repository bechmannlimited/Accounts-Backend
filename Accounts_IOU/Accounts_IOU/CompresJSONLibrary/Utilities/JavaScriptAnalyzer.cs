using Jint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CompresJSON
{
    public class JavaScriptAnalyzer
    {
        private static string setupScript()
        {
            string script = "";
            //string path = Scripts.Url("~/Scripts/compresjson").ToString();
            StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Scripts/compresjson_scripts/encryptor_compressor.js"); // + "/Scripts/compresjson_scripts/encryptor_compressor.js");
            script = streamReader.ReadToEnd();
            streamReader.Close();
            
            return script;
        }

        public static object runJavaScriptFunctionWithArgs(string functionName, Dictionary<string, object> args)
        {
            string script = setupScript();

            var js = new Engine();
            string argsString = "";

            int c = 0;
            foreach (var item in args)
            {
                js.SetValue(item.Key, item.Value);
                argsString += (c > 0 ? ", " : "") + item.Key;
                c++;
            }

            var x = functionName + "(" + argsString + ");";

            return js.Execute(script + " " + functionName + "(" + argsString + ");").GetCompletionValue().ToObject();
        }

    }
}