using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompresJSON
{
    public class DateFormatter
    {
        public static string Date = "dd/MM/yyyy";
        public static string DateTime = "dd/MM/yyyy HH:mm";
        public static string DateTimeWithSeconds = "dd/MM/yyyy HH:mm:ss";
        //case ISO8601 = "yyyy-MM-dd'T'HH:mm:ssZZZZZ"
        public static string ISO8601 = "yyyy-MM-dd'T'HH:mm:ss";
        //case ISO8601 = "yyyy-MM-dd'T'HH:mm:ss"
    }
}