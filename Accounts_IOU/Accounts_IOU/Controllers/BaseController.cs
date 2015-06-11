using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounts_IOU.Controllers
{
    public class BaseController : Controller
    {
        public Accounts_IOUEntities db = new Accounts_IOUEntities();
        public Accounts_IOUEntities jsonDB
        {
            get
            {
                return Accounts_IOUEntities.jsonDB();
            }
        }

        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
    }
}