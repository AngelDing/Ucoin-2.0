using System;
using System.Collections.Generic;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.Test
{
    public interface ICustomerRepository : IRepository<EFCustomer, Guid>
    {
        EFCustomer GetCustomerByName(string name);

        List<EFNote> GetNoteList();
    }
}
