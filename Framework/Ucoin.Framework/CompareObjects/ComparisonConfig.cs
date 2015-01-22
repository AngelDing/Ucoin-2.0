using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ucoin.Framework.CompareObjects
{
    public class ComparisonConfig
    {
        public ComparisonConfig()
        {
            TreatStringEmptyAndNullTheSame = true;
            MembersToIgnore = new List<string>();
            MembersToInclude = new List<string>();
            AttributesToIgnore = new List<Type> 
            {
                typeof(CompareIgnoreAttribute)
            };
            CollectionMatchingSpec = new Dictionary<Type, IEnumerable<string>>();
        }

        /// <summary>
        /// 默認為True
        /// </summary>
        public bool TreatStringEmptyAndNullTheSame { get; set; }

        public List<string> MembersToIgnore { get; set; }

        public List<string> MembersToInclude { get; set; }

        public List<Type> AttributesToIgnore { get; set; }

        /// <summary>
        /// 手動指定集合對象的key字段
        /// </summary>
        public Dictionary<Type, IEnumerable<string>> CollectionMatchingSpec { get; set; }
    }
}
