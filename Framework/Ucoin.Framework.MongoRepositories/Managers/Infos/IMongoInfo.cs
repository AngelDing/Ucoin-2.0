using System.Collections.Generic;

namespace Ucoin.Framework.MongoRepository.Manager
{
    public interface IMongoInfo
    {
        List<TreeNode> GetInfo();
    }
}
