using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Ucoin.Framework.ObjectMapper
{
    internal static class ExecutorFactory<TSource, TTarget>
    {
        #region Fields

        private static readonly ConcurrentDictionary<ObjectMapper, Func<TSource, TTarget>> _converters =
            new ConcurrentDictionary<ObjectMapper, Func<TSource, TTarget>>();

        private static readonly ConcurrentDictionary<ObjectMapper, Action<TSource, TTarget>> _mappers =
            new ConcurrentDictionary<ObjectMapper, Action<TSource, TTarget>>();

        #endregion

        #region Entry Points

        public static Func<TSource, TTarget> GetConverter(ObjectMapper container)
        {
            return _converters.GetOrAdd(container, CreateConverter);
        }

        public static Action<TSource, TTarget> GetMapper(ObjectMapper container)
        {
            return _mappers.GetOrAdd(container, CreateMapper);
        }

        #endregion

        private static Func<TSource, TTarget> CreateConverter(ObjectMapper container)
        {
            Func<TSource, TTarget> converter;
            if (container.Converters.Get<TSource, TTarget>() == null &&
                (TryGetArrayConverter(container, out converter) ||
                 TryGetListConverter(container, out converter) ||
                 TryGetEnumerableConverter(container, out converter) ||
                 TryGetCollectionConverter(container, out converter)))
            {
                return converter;
            }
            InstanceMapper<TSource, TTarget> mapper = InstanceMapper<TSource, TTarget>.GetInstance(container);
            return mapper.Map;
        }

        private static Action<TSource, TTarget> CreateMapper(ObjectMapper container)
        {
            Type sourceEnumerableType, targetEnumerableType;
            if (typeof(TSource).ImplementsGeneric(typeof(IEnumerable<>), out sourceEnumerableType) &&
                typeof(TTarget).ImplementsGeneric(typeof(IEnumerable<>), out targetEnumerableType))
            {
                Type sourceElementType = sourceEnumerableType.GetGenericArguments()[0];
                Type targetElementType = targetEnumerableType.GetGenericArguments()[0];
                return
                    (source, target) =>
                        container.FastInvoke(new[] { sourceElementType, targetElementType }, "Map", source, target);
            }
            InstanceMapper<TSource, TTarget> mapper = InstanceMapper<TSource, TTarget>.GetInstance(container);
            return mapper.Map;
        }

        private static bool TryGetEnumerableConverter(ObjectMapper container, out Func<TSource, TTarget> converter)
        {
            converter = null;
            Type sourceEnumerableType;
            if (typeof(TSource).ImplementsGeneric(typeof(IEnumerable<>), out sourceEnumerableType) &&
                typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type sourceElementType = sourceEnumerableType.GetGenericArguments()[0];
                Type targetElementType = typeof(TTarget).GetGenericArguments()[0];
                converter =
                    source =>
                        (TTarget)container.FastInvoke(new[] { sourceElementType, targetElementType }, "Map", source);
                return true;
            }
            return false;
        }

        private static bool TryGetArrayConverter(ObjectMapper container, out Func<TSource, TTarget> converter)
        {
            converter = null;
            Type sourceEnumerableType;
            if (typeof(TSource).ImplementsGeneric(typeof(IEnumerable<>), out sourceEnumerableType) &&
                (typeof(TTarget).IsArray || (typeof(TTarget).IsGenericType &&
                                              typeof(TTarget).GetGenericTypeDefinition() == typeof(ICollection<>)) ||
                 (typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(IList<>))))
            {
                Type sourceElementType = sourceEnumerableType.GetGenericArguments()[0];
                Type targetElementType = typeof(TTarget).IsArray
                    ? typeof(TTarget).GetElementType()
                    : typeof(TTarget).GetGenericArguments()[0];
                converter = source =>
                {
                    if (ReferenceEquals(source, null))
                    {
                        return default(TTarget);
                    }
                    object elements = container.FastInvoke(new[] { sourceElementType, targetElementType },
                        "Map", source);
                    return (TTarget)typeof(Enumerable).FastInvoke(new[] { targetElementType }, "ToArray", elements);
                };
                return true;
            }
            return false;
        }

        private static bool TryGetListConverter(ObjectMapper container, out Func<TSource, TTarget> converter)
        {
            converter = null;
            Type sourceEnumerableType;
            if (typeof(TSource).ImplementsGeneric(typeof(IEnumerable<>), out sourceEnumerableType) &&
                typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(List<>))
            {
                Type sourceElementType = sourceEnumerableType.GetGenericArguments()[0];
                Type targetElementType = typeof(TTarget).GetGenericArguments()[0];
                converter = source =>
                {
                    if (ReferenceEquals(source, null))
                    {
                        return default(TTarget);
                    }
                    object elements = container.FastInvoke(new[] { sourceElementType, targetElementType },
                        "Map", source);
                    return (TTarget)typeof(Enumerable).FastInvoke(new[] { targetElementType }, "ToList", elements);
                };
                return true;
            }
            return false;
        }

        private static bool TryGetCollectionConverter(ObjectMapper container, out Func<TSource, TTarget> converter)
        {
            converter = null;
            Type sourceEnumerableType;
            Type targetEnumerableType;
            if (typeof(TSource).ImplementsGeneric(typeof(IEnumerable<>), out sourceEnumerableType) &&
                typeof(TTarget).ImplementsGeneric(typeof(IEnumerable<>), out targetEnumerableType))
            {
                Type sourceElementType = sourceEnumerableType.GetGenericArguments()[0];
                Type targetElementType = targetEnumerableType.GetGenericArguments()[0];
                ConstructorInfo enumerableCtor =
                    typeof(TTarget).GetConstructor(new[] { typeof(IEnumerable<>).MakeGenericType(targetElementType) });
                if (enumerableCtor != null)
                {
                    converter = source =>
                    {
                        if (ReferenceEquals(source, null))
                        {
                            return default(TTarget);
                        }
                        return
                            (TTarget)
                                Activator.CreateInstance(typeof(TTarget),
                                    container.FastInvoke(new[] { sourceElementType, targetElementType }, "Map",
                                        source));
                    };
                    return true;
                }
                ConstructorInfo arrayCtor =
                    typeof(TTarget).GetConstructor(new[] { typeof(ICollection<>).MakeGenericType(targetElementType) }) ??
                    typeof(TTarget).GetConstructor(new[] { typeof(IList<>).MakeGenericType(targetElementType) }) ??
                    typeof(TTarget).GetConstructor(new[] { targetElementType.MakeArrayType() });
                if (arrayCtor != null)
                {
                    converter = source =>
                    {
                        if (ReferenceEquals(source, null))
                        {
                            return default(TTarget);
                        }
                        object elements = container.FastInvoke(new[] { sourceElementType, targetElementType },
                            "Map", source);
                        object array = typeof(Enumerable).FastInvoke(new[] { targetElementType }, "ToArray", elements);
                        return (TTarget)Activator.CreateInstance(typeof(TTarget), array);
                    };
                    return true;
                }
                ConstructorInfo listCtor =
                    typeof(TTarget).GetConstructor(new[] { typeof(List<>).MakeGenericType(targetElementType) });
                if (listCtor != null)
                {
                    converter = source =>
                    {
                        if (ReferenceEquals(source, null))
                        {
                            return default(TTarget);
                        }
                        object elements = container.FastInvoke(new[] { sourceElementType, targetElementType },
                            "Map", source);
                        object list = typeof(Enumerable).FastInvoke(new[] { targetElementType }, "ToList", elements);
                        return (TTarget)Activator.CreateInstance(typeof(TTarget), list);
                    };
                    return true;
                }
                ConstructorInfo defaultConstructor = typeof(TTarget).GetConstructor(Type.EmptyTypes);
                if (defaultConstructor != null && typeof(TTarget).Implements(typeof(ICollection<>)))
                {
                    converter = source =>
                    {
                        if (ReferenceEquals(source, null))
                        {
                            return default(TTarget);
                        }
                        var elements =
                            (IEnumerable)
                                container.FastInvoke(new[] { sourceElementType, targetElementType }, "Map",
                                    source);
                        object target = Activator.CreateInstance(typeof(TTarget));
                        foreach (object element in elements)
                        {
                            target.FastInvoke("Add", element);
                        }
                        return (TTarget)target;
                    };
                    return true;
                }
            }
            return false;
        }
    }
}