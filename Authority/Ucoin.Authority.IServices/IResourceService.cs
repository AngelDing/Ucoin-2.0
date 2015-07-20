using System.Collections.Generic;
using System.Threading.Tasks;
using Ucoin.Identity.DataObjects;

namespace Ucoin.Authority.IServices
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceInfo>> GetResourceListByUserName(string userName);

        Task<IEnumerable<ActionInfo>> GetResourceActionsByResourceId(int resourceId);
    }
}
