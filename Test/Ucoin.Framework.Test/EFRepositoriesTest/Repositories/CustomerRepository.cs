using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Framework.Test
{
    public class CustomerRepository : EfRepository<EFCustomer, int>, ICustomerRepository
    {
        public CustomerRepository()
            : base(new EfRepositoryContext(new EFTestContext()))
        { 
        }

        public EFCustomer GetCustomFullInfo(int id)
        {
            var db = DbContext as EFTestContext;
            var customer = db.EFCustomer
                .Where(p => p.Id == id)
                .Include(p => p.EFNote.Select(c => c.ChildNote))
                //.IncludeExpand(p => p.EFNote)
                //.IncludeExpand(p => p.EFNote.FirstOrDefault().ChildNote)
                .FirstOrDefault();
            return customer;
        }

        public EFCustomer GetCustomerByName(string name)
        {
            var customer = DbContext.Set<EFCustomer>()
                .Where(p => p.UserName == name)
                .Include(p => p.EFNote).First();
            return customer;
        }

        public List<EFNote> GetNoteList()
        {
            var list = DbContext.Set<EFNote>().ToList();
            return list;
        }
    }
}
