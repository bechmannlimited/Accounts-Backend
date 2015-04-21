using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounts_IOU.Controllers
{
    public class TransactionController : BaseController
    {
        // GET: Payment
        public JsonResult AddTransaction(Transaction transaction)
        {
            transaction.DateEntered = DateTime.Now;

            db.Transactions.Add(transaction);
            db.SaveChanges();

            return Json(transaction);
        }

        public JsonResult DifferenceBetweenUsers(User u, long relationUserID)
        {
            double difference = 0;

            db.Transactions.Where(x => x.UserID == u.UserID).ToList().ForEach(x => 
                difference += (double)x.Amount
            );

            db.Transactions.Where(x => x.UserID == relationUserID).ToList().ForEach(x => 
                difference -= (double)x.Amount
            );

            var response = new ResponseDictionary(ResponseStatus.Success);
            response["difference"] = difference;

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}