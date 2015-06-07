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
    public class UsersController : ApiController
    {
        private Accounts_IOUEntities db = Accounts_IOUEntities.jsonDB();

        // GET api/Users
        [Queryable]
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            //TEST BY TORBEN
            User user = db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            var friendRelations = new Accounts_IOUEntities().Relations.Where(x => x.UserID == id).ToList();
            user.Friends = Accounts_IOUEntities.jsonDB().Users.ToList().Where(x => friendRelations.Any(r => r.RelationUserID == x.UserID)).ToList();

            return Ok(user);
        }

        // PUT api/Users/5
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }

        [HttpPost]
        [Route("api/Users/login")]
        public IHttpActionResult PostLogin([FromUri]User u)
        {
            if (u != null)
            {
                var matches = db.Users.Where(x => x.Username == u.Username && x.Password == u.Password).ToList();

                if (matches.Count() > 0)
                {
                    var match = matches.FirstOrDefault();
                    var user = Accounts_IOUEntities.jsonDB().Users.Where(x => x.UserID == match.UserID).ToList().FirstOrDefault();
                    user.Password = "";
                    return Ok(user);
                }
            }

            ModelState.AddModelError("Error", new Exception("Login failed"));
            return BadRequest(ModelState);
        }

        //public JsonResult FriendInvitations(User u)
        //{
        //    User user = db.Users.Find(u.UserID);

        //    List<Relation> unconfirmedInvitations = new List<Relation>();

        //    List<Relation> invites = db.Relations.Where(r => r.RelationUserID == user.UserID).ToList();

        //    foreach (var item in invites)
        //    {
        //        var matches = db.Relations.Where(r => r.UserID == u.UserID && r.RelationUserID == item.UserID);

        //        if (matches.Count() == 0)
        //        {
        //            unconfirmedInvitations.Add(item);
        //        }
        //    }

        //    List<Relation> rc = Accounts_IOUEntities.jsonDB().Relations.ToList().Where(x => unconfirmedInvitations.Any(i => i.RelationID == x.RelationID)).ToList();
        //    rc.ForEach(x => x.User = Accounts_IOUEntities.jsonDB().Users.Find(x.UserID));

        //    return Json(rc);
        //}

        //public JsonResult ActiveUsersMatching(User u, string searchText)
        //{
        //    var user = db.Users.Find(u.UserID);

        //    searchText = searchText.ToLower();
        //    var matches = Accounts_IOUEntities.jsonDB().Users.Where(x => x.Username.ToLower() == searchText || x.Email.ToLower() == searchText).ToList();

        //    matches = matches.Where(x => x.UserID != user.UserID).ToList();
        //    matches = matches.Where(x => !user.GetJSONFriendlyListOfFriendsWithStatus().Any(f => f.UserID == x.UserID)).ToList();

        //    return Json(matches, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetFriends(User u)
        //{
        //    var user = db.Users.Find(u.UserID);

        //    //var friendRelations = new Accounts_IOUEntities().Relations.Where(x => x.UserID == user.UserID).ToList();
        //    //user.Friends = Accounts_IOUEntities.jsonDB().Users.ToList().Where(x => friendRelations.Any(r => r.RelationUserID == x.UserID)).ToList();

        //    return Json(user.GetJSONFriendlyListOfFriendsWithStatus(), JsonRequestBehavior.AllowGet);
        //}
    }

}

