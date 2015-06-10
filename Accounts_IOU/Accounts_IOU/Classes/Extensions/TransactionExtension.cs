using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts_IOU
{
    public partial class Transaction
    {

        public Purchase Purchase = new Purchase();
        //public List<Transaction> Transactions;
        //{
        //    get
        //    {
        //        return Accounts_IOUEntities.jsonDB().Transactions.Where(x => x.PurchaseID == this.PurchaseID).ToList();
        //    }
            
        //}
    }
}