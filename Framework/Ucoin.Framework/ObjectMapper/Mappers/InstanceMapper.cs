using System;
using System.Data;
using System.Collections.Concurrent;

namespace Ucoin.Framework.ObjectMapper
{
    internal class InstanceMapper<TSource, TTarget> : IInstanceMapper<TSource, TTarget>
    {
        private readonly Func<TSource, TTarget> _converter;
        private readonly Action<TSource, TTarget> _mapper;

        private static readonly ConcurrentDictionary<ObjectMapper, InstanceMapper<TSource, TTarget>> _mappers =
            new ConcurrentDictionary<ObjectMapper, InstanceMapper<TSource, TTarget>>();

        internal static InstanceMapper<TSource, TTarget> GetInstance(ObjectMapper container)
        {
            return _mappers.GetOrAdd(container, CreateMapper);
        }

        private static InstanceMapper<TSource, TTarget> CreateMapper(ObjectMapper container)
        {
            return new InstanceMapper<TSource, TTarget>(container);
        }

        private InstanceMapper(ObjectMapper container)
        {
            _converter = container.GetMapFunc<TSource, TTarget>();
            _mapper = container.GetMapAction<TSource, TTarget>();
        }

        public TTarget Map(TSource source)
        {
            if (ReferenceEquals(source, null))
            {
                return default(TTarget);
            }
            return _converter(source);
        }

        public void Map(TSource source, TTarget target)
        {
            if (!ReferenceEquals(source, null) && !ReferenceEquals(target, null))
            {
                _mapper(source, target);
            }
        }
    }
}