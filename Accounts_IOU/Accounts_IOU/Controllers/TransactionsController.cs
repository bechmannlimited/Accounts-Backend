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
        public IQueryable<Transaction> GetTransactions([FromBody]User u)
        {
            if (u.UserID > 0)
            {
                var user = db.Users.Find(u.UserID);                

                var transactions = Accounts_IOUEntities.jsonDB().Transactions.Where(x => x.UserID == u.UserID || x.RelationUserID == user.UserID);

                transactions.ToList().ForEach(x => x.User1 = Accounts_IOUEntities.jsonDB().Users.Find(x.RelationUserID));
                transactions.ToList().ForEach(x => x.User = Accounts_IOUEntities.jsonDB().Users.Find(x.UserID));
                transactions.ToList().ForEach(
                    x =>
                    x.Purchase = db.Purchases.Find(x.Purchase.PurchaseID) != null ? Accounts_IOUEntities.jsonDB().Purchases.Find(x.Purchase.PurchaseID) : new Purchase()

                    );

                return db.Transactions;
            }

            return db.Transactions;
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