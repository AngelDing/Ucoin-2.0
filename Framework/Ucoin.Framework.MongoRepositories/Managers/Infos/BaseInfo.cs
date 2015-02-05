using System.Collections.Generic;

namespace Ucoin.Framework.MongoDb.Managers
{
    public class BaseInfo : BaseContext, IMongoInfo
    {
        public virtual List<TreeNode> GetInfo()
        {
            return null;
        }
    }
}