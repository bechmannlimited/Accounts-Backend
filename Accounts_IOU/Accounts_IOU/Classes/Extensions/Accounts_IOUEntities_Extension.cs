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

        //public Accounts_IOUEntities()
        //{
        //    Configuration.LazyLoadingEnabled = false;
        //    Configuration.ProxyCreationEnabled = false;
        //}

        //public override Accounts_IOUEntities()
        //    : base("name=Accounts_IOUEntities")
        //{
        //    Configuration.LazyLoadingEnabled = false;
        //    Configuration.ProxyCreationEnabled = false;
        //}
    }
}