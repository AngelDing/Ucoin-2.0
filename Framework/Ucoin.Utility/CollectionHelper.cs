using System.Collections.Generic;

namespace Ucoin.Utility
{
    public static class CollectionHelper
    {
        public static bool HasValue<T>(this ICollection<T> input)
        {
            return !input.IsNullOrEmpty();
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> input)
        {
            if (input == null || input.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
