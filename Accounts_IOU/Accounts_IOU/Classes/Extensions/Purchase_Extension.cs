using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts_IOU
{
    public partial class Purchase
    {
        //public int[] RelationUserIDs;
        public double[] RelationUserAmounts;
        public List<User> RelationUsers = new List<User>();

        public double Amount
        {
            get
            {
                var db = Accounts_IOUEntities.jsonDB();
                var transactions = db.Transactions.Where(x => x.PurchaseID == this.PurchaseID).ToList();
                double total = 0;

                double amount = 0;

                foreach (var transaction in transactions)
                {
                    amount = (double)transaction.Amount;
                    total += amount;
                }

                total += amount; // for yourself

                return total;
            }
        }

        public void LoadTransactionData()
        {
            var ids = new List<int>();
            var amounts = new List<double>();
            //RelationUsers 

            foreach (var transaction in Accounts_IOUEntities.jsonDB().Transactions.Where(x => x.PurchaseID == this.PurchaseID).ToList())
            {
                amounts.Add((double)transaction.Amount);

                var relationUserID = (int)transaction.RelationUserID;
                var userID = (int)transaction.UserID;

                if (!ids.Contains(relationUserID))
                {
                    ids.Add(relationUserID);
                }
                if (!ids.Contains(userID))
                {
                    ids.Add(userID);
                }
            }

            foreach (var id in ids)
            {
                User user = Accounts_IOUEntities.jsonDB().Users.Find(id);
                RelationUsers.Add(user);
            }

            this.User = Accounts_IOUEntities.jsonDB().Users.Find(UserID);

            //RelationUserIDs = ids.ToArray();
            RelationUserAmounts = amounts.ToArray();
        }

    }
}