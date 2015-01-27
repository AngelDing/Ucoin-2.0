using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal sealed class ConverterCollection
    {
        private readonly ObjectMapper _container;
        private readonly IList<Converter> _converters = new List<Converter>();
        private readonly ConcurrentDictionary<ConverterKey, Converter> _resolvedConverters =
            new ConcurrentDictionary<ConverterKey, Converter>();

        private bool _readonly;

        public ConverterCollection(ObjectMapper container)
        {
            _container = container;
        }

        private void CheckReadOnly()
        {
            if (_readonly)
            {
                throw new NotSupportedException("Collection is read-only");
            }
        }

        internal void SetReadOnly()
        {
            if (!_readonly)
            {
                _readonly = true;
            }
        }

        internal void Add(Converter converter)
        {
            CheckReadOnly();
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            converter.Container = _container;
            _converters.Add(converter);
            _resolvedConverters.Clear();
        }

        public void Add<TSource, TTarget>(Func<TSource, TTarget> expression)
        {
            Add(new LambdaConverter<TSource, TTarget>(expression));
        }

        internal void Compile(ModuleBuilder builder)
        {
            foreach (Converter converter in _converters)
            {
                converter.Compile(builder);
            }
        }

        internal void AddIntrinsic<TSource, TTarget>(Func<TSource, TTarget> expression)
        {
            Add(new LambdaConverter<TSource, TTarget>(expression) {Intrinsic = true});
        }

        internal Converter Get<TSource, TTarget>()
        {
            return Get(typeof (TSource), typeof (TTarget));
        }

        internal Converter Find(ConverterMatchContext context)
        {
            return (from converter in _converters
                let score = converter.Match(context)
                where score >= 0
                orderby score, converter.Intrinsic ? 1 : 0
                select converter).FirstOrDefault();
        }

        internal Converter Get(Type sourceType, Type targetType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException("sourceType");
            }
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            return _resolvedConverters.GetOrAdd(new ConverterKey(sourceType, targetType),
                key => Find(new ConverterMatchContext(key.SourceType, key.TargetType)));
        }
    }
}