using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Accounts_IOU;

namespace Accounts_IOU.Controllers
{
    public class TransactionsController : ApiController
    {
        private Accounts_IOUEntities db = Accounts_IOUEntities.jsonDB();

        // GET: api/Transactions
        [Queryable]
        public IQueryable<Transaction> GetTransactions(int userID)
        {
            if (userID > 0)
            {
                var user = db.Users.Find(userID);                

                var transactions = Accounts_IOUEntities.jsonDB().Transactions.Where(x => x.UserID == userID || x.RelationUserID == user.UserID);//.OrderByDescending(x => x.TransactionDate); //.Include(x => x.User).Include(x => x.User1);

                transactions.ToList().ForEach(x => x.User1 = Accounts_IOUEntities.jsonDB().Users.Find(x.RelationUserID));
                transactions.ToList().ForEach(x => x.User = Accounts_IOUEntities.jsonDB().Users.Find(x.UserID));
                transactions.ToList().ForEach(x =>
                    x.Purchase = db.Purchases.Find(x.PurchaseID) != null ? Accounts_IOUEntities.jsonDB().Purchases.Find(x.PurchaseID) : new Purchase()
                );

                return transactions;
            }

            return db.Transactions;
        }


        [Queryable]
        [Route("api/Users/TransactionsBetween/{userID}/and/{relationUserID}")]
        public IQueryable<Transaction> GetTransactionsBetweenUsers(int userID, int relationUserID)
        {
            var transactions = Accounts_IOUEntities.jsonDB().Transactions.Where(x => 
                (x.UserID == userID && x.RelationUserID == relationUserID) || 
                (x.UserID == relationUserID && x.RelationUserID == userID)); //.Include(x => x.User).Include(x => x.User1);

            transactions.ToList().ForEach(x => x.User1 = Accounts_IOUEntities.jsonDB().Users.Find(x.RelationUserID));
            transactions.ToList().ForEach(x => x.User = Accounts_IOUEntities.jsonDB().Users.Find(x.UserID));
            transactions.ToList().ForEach(x =>
                x.Purchase = db.Purchases.Find(x.PurchaseID) != null ? Accounts_IOUEntities.jsonDB().Purchases.Find(x.PurchaseID) : new Purchase()
            );

            return transactions;
        }

        [Queryable]
        [Route("api/Users/DifferenceBetween/{userID}/and/{relationUserID}")]
        public IHttpActionResult GetDifferenceBetweenUsers(int userID, int relationUserID)
        {
            if (userID > 0 && relationUserID > 0)
            {
                DifferenceBetweenUsers difference = new DifferenceBetweenUsers(userID, relationUserID);

                var rc = new Dictionary<string, object>();
                rc["Difference"] = difference.Difference;
                rc["TransactionsCount"] = difference.TransactionsCount;

                return Ok(rc);
            }
            else
            {
                ModelState.AddModelError("Error", new Exception("User and relation id's are 0"));
                return BadRequest(ModelState);
            }
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.TransactionID)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            transaction.TransactionDate = DateTime.Now; // TODO: get from instance
            transaction.DateEntered = DateTime.Now;
            transaction.PurchaseID = 0;
            transaction.Description = "";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(transaction);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transaction.TransactionID }, transaction);
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.TransactionID == id) > 0;
        }
    }
}