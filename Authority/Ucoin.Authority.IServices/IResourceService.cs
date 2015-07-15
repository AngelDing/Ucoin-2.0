using System.Collections.Generic;
using Ucoin.Identity.DataObjects;

namespace Ucoin.Authority.IServices
{
    public interface IResourceService
    {
        IEnumerable<ResourceInfo> GetResourceListByUserName(string userName);
    }
}
