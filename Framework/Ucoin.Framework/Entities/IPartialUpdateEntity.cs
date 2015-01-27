
using System.Collections.Generic;
namespace Ucoin.Framework.Entities
{
    /// <summary>
    /// 局部更新接口
    /// </summary>
    public interface IPartialUpdateEntity
    {
        /// <summary>
        /// 是否局部更新
        /// </summary>
        bool IsPartialUpdate { get; set; }

        /// <summary>
        /// 需要更新的字段及其值的集合
        /// </summary>
        Dictionary<string, object> NeedUpdateList { get; }
    }
}
