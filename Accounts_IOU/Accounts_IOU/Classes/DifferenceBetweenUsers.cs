using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts_IOU
{
    public class DifferenceBetweenUsers
    {
        public double Difference { get; set; }
        public int TransactionsCount { get; set; }

        public DifferenceBetweenUsers(int userID, int relationUserID)
        {
            List<Transaction> plusTransactions = Accounts_IOUEntities.jsonDB().Transactions.Where(x => x.UserID == userID && x.RelationUserID == relationUserID).ToList();
            List<Transaction> minusTransactions = Accounts_IOUEntities.jsonDB().Transactions.Where(x => x.UserID == relationUserID && x.RelationUserID == userID).ToList();

            plusTransactions.ForEach(x =>
                Difference += (double)x.Amount
            );

            minusTransactions.ForEach(x =>
                Difference -= (double)x.Amount
            );

            Difference = Math.Round(Difference, 2);
            TransactionsCount = plusTransactions.Count() + minusTransactions.Count();
        }
    }
}