using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompresJSON
{
    public class CompresJSON
    {
        public static string EncryptAndCompressAsNecessary(string str)
        {
            //compress

            if (CompresJSONSettings.ShouldCompress)
            {
                str = Compressor.Compress(str);
            }

            //encrypt
            if (CompresJSONSettings.ShouldEncrypt)
            {
                str = Encryptor.Encrypt(str);
            }


            return str;
        }

        public static string DecryptAndDecompressAsNecessary(string str)
        {
            //decrypt
            if (CompresJSONSettings.ShouldEncrypt)
            {
                str = Encryptor.Decrypt(str);
            }

            //decompress
            if (CompresJSONSettings.ShouldCompress)
            {
                str = Compressor.Decompress(str);
            }

            return str;
        }
    }
}