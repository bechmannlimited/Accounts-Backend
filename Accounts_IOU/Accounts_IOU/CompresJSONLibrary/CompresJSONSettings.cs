using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CompresJSON
{
    public class CompresJSONSettings
    {
        public static string EncryptionKey = "7e4bac048ef766e83f0ec8c079e1f90c2eb690a9";

        public static bool ShouldCompress = true;
        public static bool ShouldEncrypt = true;

        public static CompressionMethod CompressionMethod = CompressionMethod.LZString;
        public static string DateFormat = DateFormatter.Date;
    }
}