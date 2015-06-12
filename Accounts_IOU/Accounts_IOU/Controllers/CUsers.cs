using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CompresJSON;

namespace Accounts_IOU.Controllers
{
    [EncryptAndCompressAsNecessaryWebApi]
    [DecryptAndDecompressAsNecessaryWebApi]
    public class CUsers: UsersController
    {
    }
}