using System.Collections.Generic;

namespace Ucoin.Framework.MongoRepository.Manager
{
    public class BaseInfo : BaseContext, IMongoInfo
    {
        public virtual List<TreeNode> GetInfo()
        {
            return null;
        }
    }
}