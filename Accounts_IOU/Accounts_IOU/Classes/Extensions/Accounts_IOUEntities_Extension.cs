using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounts_IOU
{
    partial class Accounts_IOUEntities
    {

        public static Accounts_IOUEntities jsonDB()
        {
            var dbLite = new Accounts_IOUEntities();
            dbLite.Configuration.LazyLoadingEnabled = false;
            dbLite.Configuration.ProxyCreationEnabled = false;
            return dbLite;
        }
    }
}