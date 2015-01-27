using System;
using System.Collections.Generic;

namespace Ucoin.Framework.ObjectMapper
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<TResult> MergeBalanced<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            using (IEnumerator<TFirst> iteratorVariable0 = first.GetEnumerator())
            {
                using (IEnumerator<TSecond> iteratorVariable1 = second.GetEnumerator())
                {
                    while (iteratorVariable0.MoveNext())
                    {
                        if (!iteratorVariable1.MoveNext())
                        {
                            throw new InvalidOperationException("Second sequence ran out before first");
                        }
                        yield return resultSelector(iteratorVariable0.Current, iteratorVariable1.Current);
                    }
                    if (iteratorVariable1.MoveNext())
                    {
                        throw new InvalidOperationException("First sequence ran out before second");
                    }
                }
            }
        }

        public static IEnumerable<TResult> Merge<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            using (IEnumerator<TFirst> iteratorVariable0 = first.GetEnumerator())
            {
                using (IEnumerator<TSecond> iteratorVariable1 = second.GetEnumerator())
                {
                    while (iteratorVariable0.MoveNext())
                    {
                        if (!iteratorVariable1.MoveNext())
                        {
                            break;
                        }
                        yield return resultSelector(iteratorVariable0.Current, iteratorVariable1.Current);
                    }
                }
            }
        }

        /// <summary>
        ///     Performs an action for each item in the enumerable
        /// </summary>
        /// <typeparam name="T">The enumerable data type</typeparam>
        /// <param name="values">The data values.</param>
        /// <param name="action">The action to be performed.</param>
        /// <remarks>
        ///     This method was intended to return the passed values to provide method chaining.
        ///     However due to defered execution the compiler would actually never run the entire code at all.
        /// </remarks>
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (T value in values)
            {
                action(value);
            }
        }
    }
}