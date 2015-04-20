using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Accounts_IOU.Controllers
{

    public class UserController : Controller
    {
        Accounts_IOUEntities db = new Accounts_IOUEntities();

        //
        // GET: /User/
        public JsonResult AddFriend(User u, long relationUserID)
        {
            Relation relation = new Relation();
            User user = db.Users.Find(u.UserID);
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
            return Json(rc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Login(string username, string password)
        {
            var matches = db.Users.Where(x => x.Username == username && x.Password == password).ToList();

            var success = matches.Count() > 0 ? ResponseStatus.Success : ResponseStatus.Failed;
            var response = new ResponseDictionary(success);

            if (matches.Count() > 0)
	        {
		        var match = matches.FirstOrDefault();
                var user = Accounts_IOUEntities.jsonDB().Users.Where(x => x.UserID == match.UserID).ToList().FirstOrDefault();

                //var friendRelations = new Accounts_IOUEntities().Relations.Where(x => x.UserID == user.UserID).ToList();
                //user.Friends = Accounts_IOUEntities.jsonDB().Users.ToList().Where(x => friendRelations.Any(r => r.RelationUserID == x.UserID)).ToList();

                response["User"] = user;
	        }

           

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FriendInvitations(User u)
        {
            User user = db.Users.Find(u.UserID);

            List<Relation> unconfirmedInvitations = new List<Relation>();

            List<Relation> invites = db.Relations.Where(r => r.RelationUserID == user.UserID).ToList();

            foreach (var item in invites)
	        {
                var matches = db.Relations.Where(r => r.UserID == u.UserID && r.RelationUserID == item.UserID);

                if (matches.Count() == 0)
                {
                    unconfirmedInvitations.Add(item);
                }
	        }

            List<Relation> rc = Accounts_IOUEntities.jsonDB().Relations.ToList().Where(x => unconfirmedInvitations.Any(i => i.RelationID == x.RelationID)).ToList();
            rc.ForEach(x => x.User = Accounts_IOUEntities.jsonDB().Users.Find(x.UserID));

            return Json(rc);
        }

        public JsonResult ActiveUsersMatching(User u, string searchText)
        {
            var user = db.Users.Find(u.UserID);

            searchText = searchText.ToLower();
            var matches = Accounts_IOUEntities.jsonDB().Users.Where(x => x.Username.ToLower() == searchText || x.Email.ToLower() == searchText).ToList();

            matches = matches.Where(x => x.UserID != user.UserID).ToList();
            matches = matches.Where(x => !user.GetJSONFriendlyListOfFriendsWithStatus().Any(f => f.UserID == x.UserID)).ToList();

            return Json(matches, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFriends(User u)
        {
            var user = db.Users.Find(u.UserID);

            //var friendRelations = new Accounts_IOUEntities().Relations.Where(x => x.UserID == user.UserID).ToList();
            //user.Friends = Accounts_IOUEntities.jsonDB().Users.ToList().Where(x => friendRelations.Any(r => r.RelationUserID == x.UserID)).ToList();

            return Json(user.GetJSONFriendlyListOfFriendsWithStatus(), JsonRequestBehavior.AllowGet);
        }
	}
}