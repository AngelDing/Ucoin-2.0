using System.Linq;
using System.Reflection;

namespace Ucoin.Framework.CompareObjects
{
    internal static class ExcludeLogic 
    {
        public static bool ShouldExcludeMember(ComparisonConfig config, MemberInfo info)
        {
            if (config.MembersToInclude.Count > 0 && !config.MembersToInclude.Contains(info.Name))
            {
                return true;
            }

            if (config.MembersToIgnore.Count > 0 && config.MembersToIgnore.Contains(info.Name))
            {
                return true;
            }

            if (IgnoredByAttribute(config, info))
            {
                return true;
            }

            return false;
        }

        private static bool IgnoredByAttribute(ComparisonConfig config, MemberInfo info)
        {
            var attributes = info.GetCustomAttributes(true);

            return attributes.Any(a => config.AttributesToIgnore.Contains(a.GetType()));
        }
    }
}
