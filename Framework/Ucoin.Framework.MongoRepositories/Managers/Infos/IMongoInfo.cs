using System.Collections.Generic;

namespace Ucoin.Framework.MongoDb.Managers
{
    public interface IMongoInfo
    {
        List<TreeNode> GetInfo();
    }
}
