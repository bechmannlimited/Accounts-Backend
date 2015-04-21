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
            transaction.TransactionDate = DateTime.Now; // TODO: get from instance
            transaction.DateEntered = DateTime.Now;
            transaction.PurchaseID = 0;
            transaction.Description = "";
            
            db.Transactions.Add(transaction);
            db.SaveChanges();

            return Json(transaction);
        }

        public JsonResult AddPurchase(Purchase purchase, int[] relationUserIDs, double[] relationUserAmounts)
        {
            ResponseStatus status = ResponseStatus.Failed;
            string message = "";

            try
            {
                purchase.DatePurchased = DateTime.Now; // TODO: get from instance
                purchase.DateEntered = DateTime.Now;

                db.Purchases.Add(purchase);
                db.SaveChanges();

                for (int i = 0; i < relationUserIDs.Length; i++)
			    {
                    int relationUserID = relationUserIDs[i];
                    double amount = relationUserAmounts[i];

			        Transaction transaction = new Transaction();
                    transaction.UserID = (int)purchase.UserID;
                    transaction.RelationUserID = relationUserID;
                    transaction.Amount = amount;
                    db.Transactions.Add(transaction);
			    }

                db.SaveChanges();
                status = ResponseStatus.Success;
            }
            catch (Exception e)
            {
                message = e.Message;
                
            }

            var response = new ResponseDictionary(status);

            if (message.Length > 0)
            {
                response["Message"] = message;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DifferenceBetweenUsers(User u, long relationUserID)
        {
            double difference = 0;

            db.Transactions.Where(x => x.UserID == u.UserID && x.RelationUserID == relationUserID).ToList().ForEach(x => 
                difference += (double)x.Amount
            );

            db.Transactions.Where(x => x.UserID == relationUserID && x.RelationUserID == u.UserID).ToList().ForEach(x => 
                difference -= (double)x.Amount
            );

            var response = new ResponseDictionary(ResponseStatus.Success);
            response["difference"] = difference;

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}