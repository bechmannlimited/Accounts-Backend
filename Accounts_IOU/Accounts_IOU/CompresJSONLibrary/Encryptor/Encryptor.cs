using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompresJSON
{
    public class Encryptor
    {

        public static string Encrypt(string str)
        {
            var p = new CryptoJS();
            var encrypted = p.OpenSSLEncrypt(str, CompresJSONSettings.EncryptionKey);
            return Converter.Base64Encode(encrypted);
        }

        public static string Decrypt(string str)
        {
            var p = new CryptoJS();
            var decoded = Converter.Base64Decode(str);
            return p.OpenSSLDecrypt(decoded, CompresJSONSettings.EncryptionKey);
        }
      

    }
}