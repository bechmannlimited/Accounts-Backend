using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompresJSON
{
    public class CompresJSON
    {
        public static string EncryptAndCompress(string str, bool shouldEncrypt, bool shouldCompress)
        {
            //compress

            if (shouldCompress)
            {
                str = Compressor.Compress(str);
            }

            //encrypt
            if (shouldEncrypt)
            {
                str = Encryptor.Encrypt(str);
            }


            return str;
        }

        public static string DecryptAndDecompress(string str, bool shouldEncrypt, bool shouldCompress)
        {
            //decrypt
            if (shouldEncrypt)
            {
                str = Encryptor.Decrypt(str);
            }

            //decompress
            if (shouldCompress)
            {
                str = Compressor.Decompress(str);
            }

            return str;
        }
    }
}