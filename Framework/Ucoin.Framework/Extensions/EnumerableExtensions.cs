using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Ucoin.Framework.Utils;

namespace Ucoin.Framework.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasItems(this IEnumerable source)
        {
            return source != null && source.GetEnumerator().MoveNext();
        }

        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            return !HasItems(source);
        }
    }
}