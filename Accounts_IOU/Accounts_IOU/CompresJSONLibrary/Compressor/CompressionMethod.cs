using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompresJSON
{
    public enum CompressionMethod
    {
        None,
        GZip,
        LZ77,
        LZString,
        GZipAndLZString
    };
}
