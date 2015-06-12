using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace CompresJSON
{
    public class Converter
    {

        public static byte[] StringToBytes(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string BytesToString(byte[] bytes)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(bytes);
        }

        public static Dictionary<string, string> QueryStringToDictionary(string str)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(str);
            return Tools.ToDictionary<string, string>(nvc);
        }

        public string StringToBase64String(string str)
        {
            var data = StringToBytes(str);
            return Convert.ToBase64String(data);
        }

        public string Base64StringFromString(string str) {
            var data = Convert.FromBase64String(str);
            return BytesToString(data);
        }

        public static string Base64Encode(string plainText) {
          var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
          return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData) {
          var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
          return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }

}




