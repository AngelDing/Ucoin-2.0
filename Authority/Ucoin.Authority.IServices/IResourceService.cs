using System.Collections.Generic;

namespace Ucoin.Authority.IServices
{
    public interface IResourceService
    {
        IEnumerable<dynamic> GetUserResources(string userName);
    }
}
