using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Xunit;
using Ucoin.Framework.EfExtensions;

namespace Ucoin.EfExtensions.Test
{

    public class ExtensionTest
    {
        [Fact]
        public void BeginTransactionObjectContext()
        {
            using (var db = new TrackerContext())
            using (var tx = db.Database.BeginTransaction())
            {
                string emailDomain = "@test.com";

                var query =  db.Users.Where(u => u.EmailAddress.Contains(emailDomain));
                int count = query.Update(u => new User { IsApproved = false, LastActivityDate = DateTime.Now });

                count = db.Tasks
                  .Where(u => u.Id == 1)
                  .Update(t => new Task { Summary = "XXX" });

                tx.Commit();
            }
        }

        [Fact]
        public void NoTransactionObjectContext()
        {
            using (var db = new TrackerContext())
            {
                string emailDomain = "@test.com";

                int count = db.Users
                    .Where(u => u.EmailAddress.EndsWith(emailDomain))
                    .Update(u => new User { IsApproved = false, LastActivityDate = DateTime.Now });

                count = db.Tasks
                  .Where(u => u.Id == 1)
                  .Update(t => new Task { Summary = "YYY" });
            }
        }

        [Fact]
        public void TransactionScopeObjectContext()
        {
            using (var tx = new TransactionScope())
            using (var db = new TrackerContext())
            {
                string emailDomain = "@test.com";

                int count = db.Users
                    .Where(u => u.EmailAddress.EndsWith(emailDomain))
                    .Update(u => new User { IsApproved = false, LastActivityDate = DateTime.Now });

                count = db.Tasks
                    .Where(u => u.Id == 1)
                    .Update(t => new Task { Summary = "ZZZ" });

                tx.Complete();
            }
        }
    }
}
