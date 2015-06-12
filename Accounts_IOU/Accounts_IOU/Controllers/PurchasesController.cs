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
using CompresJSON;

namespace Accounts_IOU.Controllers
{
    [DecryptAndDecompressAsNecessaryWebApi]
    [EncryptAndCompressAsNecessaryWebApi]
    public class PurchasesController : ApiController
    {
        private Accounts_IOUEntities db = Accounts_IOUEntities.jsonDB();

        // GET: api/Purchases
        public IQueryable<Purchase> GetPurchases()
        {
            return db.Purchases;
        }

        // GET: api/Purchases/5
        [ResponseType(typeof(Purchase))]
        public IHttpActionResult GetPurchase(int id)
        {
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return NotFound();
            }

            purchase.LoadTransactionData();

            return Ok(purchase);
        }

        // PUT: api/Purchases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPurchase(int id, Purchase purchase, [FromUri]int[] RelationUserIDs, [FromUri] double[] RelationUserAmounts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != purchase.PurchaseID)
            {
                return BadRequest();
            }

            db.Entry(purchase).State = EntityState.Modified;

            //UPDATE PURCHASE DETAILS 
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //REMOVE OLD TRANSACTIONS
            foreach (var transaction in db.Transactions.Where(x => x.PurchaseID == id).ToList())
            {
                db.Transactions.Remove(transaction); // IS THIS NECCESSARY? CASCADE?
            }

            db.SaveChanges();

            //ADD TRANSACTIONS AGAIN
            try
            {
                //purchase.DatePurchased = DateTime.Now; // TODO: get from instance
                //purchase.DateEntered = DateTime.Now;

                //db.Purchases.Add(purchase);
                //db.SaveChanges();

                for (int i = 0; i < RelationUserIDs.Length; i++)
                {
                    int relationUserID = RelationUserIDs[i];
                    double amount = RelationUserAmounts[i];

                    if (relationUserID != purchase.UserID) {

                        Transaction transaction = new Transaction();
                        transaction.UserID = (int)purchase.UserID;
                        transaction.RelationUserID = relationUserID;
                        transaction.PurchaseID = purchase.PurchaseID;
                        transaction.Amount = amount;
                        transaction.Description = purchase.Description;
                        transaction.TransactionDate = purchase.DatePurchased;
                        transaction.DateEntered = purchase.DateEntered;
                        db.Transactions.Add(transaction);
                    }
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e);
                return BadRequest(ModelState);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Purchases
        [ResponseType(typeof(Purchase))]
        public IHttpActionResult PostPurchase(Purchase purchase, [FromUri]int[] RelationUserIDs, [FromUri] double[] RelationUserAmounts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //purchase.DatePurchased = DateTime.Now; // TODO: get from instance
                purchase.DateEntered = DateTime.Now;

                db.Purchases.Add(purchase);
                db.SaveChanges();

                for (int i = 0; i < RelationUserIDs.Length; i++)
                {
                    int relationUserID = RelationUserIDs[i];
                    double amount = RelationUserAmounts[i];

                    if (relationUserID != purchase.UserID)
                    {
                        Transaction transaction = new Transaction();
                        transaction.UserID = (int)purchase.UserID;
                        transaction.RelationUserID = relationUserID;
                        transaction.PurchaseID = purchase.PurchaseID;
                        transaction.Amount = amount;
                        transaction.Description = purchase.Description;
                        transaction.TransactionDate = purchase.DatePurchased;
                        transaction.DateEntered = purchase.DateEntered;
                        db.Transactions.Add(transaction);
                    }
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e);
                return BadRequest(ModelState);
            }

            return CreatedAtRoute("DefaultApi", new { id = purchase.PurchaseID }, purchase);
        }

        // DELETE: api/Purchases/5
        [ResponseType(typeof(Purchase))]
        public IHttpActionResult DeletePurchase(int id)
        {
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return NotFound();
            }

            db.Purchases.Remove(purchase);
            db.SaveChanges();

            foreach (var transaction in db.Transactions.Where(x => x.PurchaseID == id).ToList())
            {
                db.Transactions.Remove(transaction); // IS THIS NECCESSARY? CASCADE?
            }

            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PurchaseExists(int id)
        {
            return db.Purchases.Count(e => e.PurchaseID == id) > 0;
        }
    }
}