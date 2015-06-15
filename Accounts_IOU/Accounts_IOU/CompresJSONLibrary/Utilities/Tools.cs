using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace CompresJSON
{
    public static class Tools
    {

        public static Dictionary<string, string> ToDictionary<TKey, TValue>(this NameValueCollection col)
        {
            var dict = new Dictionary<string, string>();

            foreach (string key in col)
            {
                dict.Add(key, col[key]);
            }

            return dict;
        }

        public static string Domain()
        {
            return RemoveFromEnd(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath, "/");
        }

        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }

        public static string GetValueFromRequestHeader(HttpRequestHeaders headers, string key) {

            string rc = "";

            try
            {
                rc = headers.Where(x => x.Key == key).FirstOrDefault().Value.FirstOrDefault();
            }
            catch (Exception)
            {

            }

            return rc;
        }
    }
}