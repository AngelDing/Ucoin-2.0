using System.Collections.Generic;
using Ucoin.Authority.Entities;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.IRepositories
{
    public interface IResourceRepositroy : IEfRepository<Resource, int>
    {
        IEnumerable<Resource> GetResourceListByUserName(string userName);
    }
}
