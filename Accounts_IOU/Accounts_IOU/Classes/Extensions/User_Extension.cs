using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts_IOU
{
    public partial class User
    {
        public RelationStatus relationStatus = RelationStatus.Undefined;
        public List<User> Friends = new List<User>();

        public List<User> GetJSONFriendlyListOfConfirmedFriends() 
        {
            var db = Accounts_IOUEntities.jsonDB();

            var friends = new List<User>();
            var userRelations = db.Relations.Where(x => x.UserID == this.UserID).ToList();

            foreach (var relation in userRelations)
            {
                if (db.Relations.Where(x => x.UserID == relation.RelationUserID && x.RelationUserID == this.UserID).Count() > 0)
                {
                    var friend = Accounts_IOUEntities.jsonDB().Users.Find(relation.RelationUserID);
                    friends.Add(friend);
                }
            }

            return friends;
        }

        public List<User> GetJSONFriendlyListOfFriendsWithStatus()
        {
            var db = Accounts_IOUEntities.jsonDB();

            var friends = new List<User>();
            var userRelations = db.Relations.Where(x => x.UserID == this.UserID).ToList();

            foreach (var relation in userRelations)
            {
                var friend = Accounts_IOUEntities.jsonDB().Users.Find(relation.RelationUserID);

                if (db.Relations.Where(x => x.UserID == relation.RelationUserID && x.RelationUserID == this.UserID).Count() > 0)
                {
                    friend.relationStatus = RelationStatus.Confirmed;
                }
                else{
                    friend.relationStatus = RelationStatus.Pending;
                }

                friends.Add(friend);
            }

            return friends;
        }
    }
}