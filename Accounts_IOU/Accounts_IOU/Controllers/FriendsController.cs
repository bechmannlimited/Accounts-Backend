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
    public class FriendsController : ApiController
    {
        private Accounts_IOUEntities db = Accounts_IOUEntities.jsonDB();

        // GET api/friends
        [Queryable]
        public IQueryable<User> Get(int userID)
        {
            User user = db.Users.Find(userID);
            return user.GetJSONFriendlyListOfFriendsWithStatus().AsQueryable();
        }

        // POST api/friends
        [ResponseType(typeof(User))]
        public IHttpActionResult Post(int userID, [FromUri]long relationUserID)
        {
            Relation relation = new Relation();
            User user = db.Users.Find(userID);
            User relationUser = db.Users.Find(relationUserID);

            if (user != null && relationUser != null)
            {
                if (!(db.Relations.Any(x => x.UserID == user.UserID && x.RelationUserID == relationUser.UserID)))
                {
                    relation.RelationUserID = relationUser.UserID;
                    relation.DateEntered = DateTime.Now;

                    user.Relations.Add(relation);

                    db.SaveChanges();
                }
                else
                {
                    relation = db.Relations.Where(x => x.UserID == user.UserID && x.RelationUserID == relationUser.UserID).FirstOrDefault();
                }
            }

            Relation rc = Accounts_IOUEntities.jsonDB().Relations.Find(relation.RelationID);
            return Ok(rc);
        }

        // PUT api/friends/5
        [ResponseType(typeof(User))]
        public void Put(int id, [FromUri]string value)
        {
        }

        // DELETE api/friends/5
        public void Delete(int id)
        {
        }
    }


}
