using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Ucoin.Framework.EFRepository;

namespace Ucoin.Framework.Test
{
    public class CustomerRepository : EFRepository<EFCustomer, Guid>, ICustomerRepository
    {
        public CustomerRepository()
            : base(new EFRepositoryContext(new EFTestContext()))
        { 
        }

        public EFCustomer GetCustomerByName(string name)
        {
            var customer = DbContext.Set<EFCustomer>()
                .Where(p => p.UserName == name)
                .Include(p => p.Notes).First();
            return customer;
        }

        public List<EFNote> GetNoteList()
        {
            var list = DbContext.Set<EFNote>().ToList();
            return list;
        }
    }
}
