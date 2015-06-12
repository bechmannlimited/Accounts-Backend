//using System;
//using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
//using System.Linq;
//using System.Web;

namespace CompresJSON
{
    public class GZip
    {
        public GZip()
        {
        }

        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzStream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gzStream.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            using (GZipStream gzStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress, true))
            {
                const int bufferSize = 4096;
                int bytesRead = 0;

                byte[] buffer = new byte[bufferSize];

                using (MemoryStream ms = new MemoryStream())
                {
                    while ((bytesRead = gzStream.Read(buffer, 0, bufferSize)) > 0)
                    {
                        ms.Write(buffer, 0, bytesRead);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}