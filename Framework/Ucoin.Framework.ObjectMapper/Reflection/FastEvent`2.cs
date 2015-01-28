using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastEvent<T, THandler>
    {
        private static readonly ConcurrentDictionary<string, FastEvent<T, THandler>> _eventCache =
            new ConcurrentDictionary<string, FastEvent<T, THandler>>();

        private readonly Lazy<Action<T, THandler>> _adder;
        private readonly Lazy<Action<T, THandler>> _remover;

        public FastEvent(EventInfo evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException("evt");
            }
            Event = evt;
            _adder = new Lazy<Action<T, THandler>>(() => GetAddMethod(evt));
            _remover = new Lazy<Action<T, THandler>>(() => GetRemoveMethod(evt));
        }

        public EventInfo Event { get; private set; }

        public void Add(T instance, THandler handler)
        {
            _adder.Value(instance, handler);
        }

        public void Remove(T instance, THandler handler)
        {
            _remover.Value(instance, handler);
        }

        private static Action<T, THandler> GetEventMethod(MethodInfo method, Type handlerType)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(THandler), "handler");

            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));

            Expression handlerCast = handlerType != typeof(THandler)
                ? Expression.TypeAs(handler, handlerType)
                : (Expression)handler;

            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, handlerCast)
                : Expression.Call(instanceCast, method, handlerCast);

            return Expression.Lambda<Action<T, THandler>>(call, instance, handler).Compile();
        }

        private static Action<T, THandler> GetAddMethod(EventInfo evt)
        {
            MethodInfo method = evt.GetAddMethod(true);
            if (method == null)
            {
                return (x, i) => { throw new InvalidOperationException("No add method available on " + evt.Name); };
            }
            return GetEventMethod(method, evt.EventHandlerType);
        }

        private static Action<T, THandler> GetRemoveMethod(EventInfo evt)
        {
            MethodInfo method = evt.GetRemoveMethod(true);
            if (method == null)
            {
                return (x, i) => { throw new InvalidOperationException("No remove method available on " + evt.Name); };
            }
            return GetEventMethod(method, evt.EventHandlerType);
        }

        public static FastEvent<T, THandler> Get(string name)
        {
            return _eventCache.GetOrAdd(name, CreateEvent);
        }

        private static FastEvent<T, THandler> CreateEvent(string name)
        {
            EventInfo info = FastEvent.GetEvent(typeof(T), name);
            return info == null ? null : new FastEvent<T, THandler>(info);
        }
    }
}