using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastEvent
    {
        private static readonly ConcurrentDictionary<TypeMemberKey, EventInfo> _eventInfoCache =
            new ConcurrentDictionary<TypeMemberKey, EventInfo>();

        private static readonly ConcurrentDictionary<TypeMemberKey, FastEvent> _eventCache =
            new ConcurrentDictionary<TypeMemberKey, FastEvent>();

        private Lazy<Action<object, Delegate>> _adder;
        private Lazy<Action<object, Delegate>> _remover;

        private FastEvent(EventInfo evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException("evt");
            }
            Event = evt;
            _adder = new Lazy<Action<object, Delegate>>(() => GetAddMethod(evt));
            _remover = new Lazy<Action<object, Delegate>>(() => GetRemoveMethod(evt));
        }

        public void Add(object instance, Delegate handler)
        {
            _adder.Value(instance, handler);
        }

        public void Remove(object instance, Delegate handler)
        {
            _remover.Value(instance, handler);
        }

        public static FastEvent Get(Type type, string name)
        {
            return _eventCache.GetOrAdd(new TypeMemberKey(type, name), key => CreateEvent(key.Type, key.MemberName));
        }

        internal static EventInfo GetEvent(Type type, string eventName)
        {
            return _eventInfoCache.GetOrAdd(new TypeMemberKey(type, eventName),
                key => FindEvent(key.Type, key.MemberName));
        }

        public EventInfo Event { get; private set; }

        private static Action<object, Delegate> GetEventMethod(MethodInfo method, Type handlerType)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(Delegate), "handler");

            // value as T is slightly faster than (T)value, so if it's not a value type, use that
            UnaryExpression instanceCast = method.DeclaringType.IsValueType
                ? Expression.Convert(instance, method.DeclaringType)
                : Expression.TypeAs(instance, method.DeclaringType);

            UnaryExpression handlerCast = Expression.TypeAs(handler, handlerType);

            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, handlerCast)
                : Expression.Call(instanceCast, method, handlerCast);

            return Expression.Lambda<Action<object, Delegate>>(call, instance, handler).Compile();
        }

        private static Action<object, Delegate> GetAddMethod(EventInfo evt)
        {
            MethodInfo method = evt.GetAddMethod(true);
            if (method == null)
            {
                return (x, i) => { throw new InvalidOperationException("No add method available on " + evt.Name); };
            }
            return GetEventMethod(method, evt.EventHandlerType);
        }

        private static Action<object, Delegate> GetRemoveMethod(EventInfo evt)
        {
            MethodInfo method = evt.GetRemoveMethod(true);
            if (method == null)
            {
                return (x, i) => { throw new InvalidOperationException("No remove method available on " + evt.Name); };
            }
            return GetEventMethod(method, evt.EventHandlerType);
        }

        private static EventInfo FindEvent(Type type, string eventName)
        {
            EventInfo evt = type.GetEvent(eventName,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (evt == null)
            {
                Type baseType = type.BaseType;
                if (baseType != null)
                {
                    evt = GetEvent(baseType, eventName);
                }
            }
            return evt;
        }

        private static FastEvent CreateEvent(Type type, string name)
        {
            EventInfo info = GetEvent(type, name);
            return info == null ? null : new FastEvent(info);
        }
    }
}