using System;
using System.Linq.Expressions;

namespace Ucoin.Framework.ObjectMapper
{
    internal static class FastExtensions
    {
        #region FastInvoke

        public static object FastInvoke(this Type type, string methodName)
        {
            return FastInvoker.Get(type).Invoke(null, methodName);
        }

        public static object FastInvoke(this Type type, string methodName, params object[] args)
        {
            return FastInvoker.Get(type).Invoke(null, methodName, args);
        }

        public static object FastInvoke(this Type type, Type[] genericTypes, string methodName)
        {
            return FastInvoker.Get(type).Invoke(null, genericTypes, methodName);
        }

        public static object FastInvoke(this Type type, Type[] genericTypes, string methodName, params object[] args)
        {
            return FastInvoker.Get(type).Invoke(null, genericTypes, methodName, args);
        }

        public static TResult FastInvoke<TResult>(this Type type, string methodName)
        {
            return (TResult) type.FastInvoke(methodName);
        }

        public static TResult FastInvoke<TResult>(this Type type, string methodName, params object[] args)
        {
            return (TResult) type.FastInvoke(methodName, args);
        }

        public static TResult FastInvoke<TResult>(this Type type, Type[] genericTypes, string methodName)
        {
            return (TResult) type.FastInvoke(genericTypes, methodName);
        }

        public static TResult FastInvoke<TResult>(this Type type, Type[] genericTypes, string methodName,
            params object[] args)
        {
            return (TResult) type.FastInvoke(genericTypes, methodName, args);
        }

        public static object FastInvoke(this object target, string methodName)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, methodName);
        }

        public static object FastInvoke(this object target, string methodName, params object[] args)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, methodName, args);
        }

        public static object FastInvoke(this object target, Type[] genericTypes, string methodName)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, genericTypes, methodName);
        }

        public static object FastInvoke(this object target, Type[] genericTypes, string methodName, params object[] args)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, genericTypes, methodName, args);
        }

        public static object FastInvoke(this object target, LambdaExpression expression)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, expression);
        }

        public static object FastInvoke(this object target, LambdaExpression expression, params object[] args)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, expression, args);
        }

        public static object FastInvoke(this object target, Type[] genericTypes, LambdaExpression expression)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, genericTypes, expression);
        }

        public static object FastInvoke(this object target, Type[] genericTypes, LambdaExpression expression,
            params object[] args)
        {
            if (target == null) throw new ArgumentNullException("target");
            return FastInvoker.Get(target.GetType()).Invoke(target, genericTypes, expression, args);
        }

        public static object FastInvoke<T>(this T target, string methodName)
        {
            return FastInvoker<T>.Current.Invoke(target, methodName);
        }

        public static object FastInvoke<T>(this T target, string methodName, params object[] args)
        {
            return FastInvoker<T>.Current.Invoke(target, methodName, args);
        }

        public static object FastInvoke<T>(this T target, Type[] genericTypes, string methodName)
        {
            return FastInvoker<T>.Current.Invoke(target, genericTypes, methodName);
        }

        public static object FastInvoke<T>(this T target, Type[] genericTypes, string methodName, params object[] args)
        {
            return FastInvoker<T>.Current.Invoke(target, genericTypes, methodName, args);
        }

        public static object FastInvoke<T>(this T target, Expression<Action<T>> expression)
        {
            return FastInvoker<T>.Current.Invoke(target, expression);
        }

        public static object FastInvoke<T>(this T target, Expression<Action<T>> expression, params object[] args)
        {
            return FastInvoker<T>.Current.Invoke(target, expression, args);
        }

        public static object FastInvoke<T>(this T target, Type[] genericTypes, Expression<Action<T>> expression)
        {
            return FastInvoker<T>.Current.Invoke(target, genericTypes, expression);
        }

        public static object FastInvoke<T>(this T target, Type[] genericTypes, Expression<Action<T>> expression,
            params object[] args)
        {
            return FastInvoker<T>.Current.Invoke(target, genericTypes, expression, args);
        }

        public static TResult FastInvoke<T, TResult>(this T target, string methodName)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, methodName);
        }

        public static TResult FastInvoke<T, TResult>(this T target, string methodName, params object[] args)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, methodName, args);
        }

        public static TResult FastInvoke<T, TResult>(this T target, Type[] genericTypes, string methodName)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, genericTypes, methodName);
        }

        public static TResult FastInvoke<T, TResult>(this T target, Type[] genericTypes, string methodName,
            params object[] args)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, genericTypes, methodName, args);
        }

        public static TResult FastInvoke<T, TResult>(this T target, Expression<Func<T, TResult>> expression)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, expression);
        }

        public static TResult FastInvoke<T, TResult>(this T target, Expression<Func<T, TResult>> expression,
            params object[] args)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, expression, args);
        }

        public static TResult FastInvoke<T, TResult>(this T target, Type[] genericTypes,
            Expression<Func<T, TResult>> expression)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, genericTypes, expression);
        }

        public static TResult FastInvoke<T, TResult>(this T target, Type[] genericTypes,
            Expression<Func<T, TResult>> expression, params object[] args)
        {
            return FastInvoker<T, TResult>.Current.Invoke(target, genericTypes, expression, args);
        }

        #endregion

        #region Fast Property

        private static FastProperty GetProperty(Type type, string name)
        {
            FastProperty property = FastProperty.Get(type, name);
            if (property == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified property '{0}' in type {1}.", name,
                    type));
            }
            return property;
        }

        private static FastProperty<T> GetProperty<T>(string name)
        {
            FastProperty<T> property = FastProperty<T>.Get(name);
            if (property == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified property '{0}' in type {1}.", name,
                    typeof (T)));
            }
            return property;
        }

        private static FastProperty<T, TResult> GetProperty<T, TResult>(string name)
        {
            FastProperty<T, TResult> property = FastProperty<T, TResult>.Get(name);
            if (property == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified property '{0}' in type {1}.", name,
                    typeof (T)));
            }
            return property;
        }

        public static object FastGetProperty(this Type type, string name)
        {
            return GetProperty(type, name).Get(null);
        }

        public static TResult FastGetProperty<TResult>(this Type type, string name)
        {
            return (TResult) type.FastGetProperty(name);
        }

        public static object FastGetProperty(this object target, string name)
        {
            if (target == null) throw new ArgumentNullException("target");
            return GetProperty(target.GetType(), name).Get(target);
        }

        public static TResult FastGetProperty<TResult>(this object target, string name)
        {
            return (TResult) target.FastGetProperty(name);
        }

        public static object FastGetProperty<T>(this T target, string name)
        {
            return GetProperty<T>(name).Get(target);
        }

        public static TResult FastGetProperty<T, TResult>(this T target, string name)
        {
            return GetProperty<T, TResult>(name).Get(target);
        }

        public static void FastSetProperty<T, TResult>(this T target, string name, TResult value)
        {
            GetProperty<T, TResult>(name).Set(target, value);
        }

        public static void FastSetProperty<T>(this T target, string name, object value)
        {
            GetProperty<T>(name).Set(target, value);
        }

        public static void FastSetProperty(this object target, string name, object value)
        {
            if (target == null) throw new ArgumentNullException("target");
            GetProperty(target.GetType(), name).Set(target, value);
        }

        public static void FastSetProperty<TResult>(this object target, string name, TResult value)
        {
            target.FastSetProperty(name, (object) value);
        }

        public static void FastSetProperty(this Type type, string name, object value)
        {
            GetProperty(type, name).Set(null, value);
        }

        public static void FastSetProperty<TResult>(this Type type, string name, TResult value)
        {
            type.FastSetProperty(name, (object) value);
        }

        #endregion

        #region Fast Field

        private static FastField GetField(Type type, string name)
        {
            FastField field = FastField.Get(type, name);
            if (field == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified field '{0}' in the type {1}.", name,
                    type));
            }
            return field;
        }

        private static FastField<T> GetField<T>(string name)
        {
            FastField<T> field = FastField<T>.Get(name);
            if (field == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified field '{0}' in the type {1}.", name,
                    typeof (T)));
            }
            return field;
        }

        private static FastField<T, TResult> GetField<T, TResult>(string name)
        {
            FastField<T, TResult> field = FastField<T, TResult>.Get(name);
            if (field == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified field '{0}' in the type {1}.", name,
                    typeof (T)));
            }
            return field;
        }

        public static object FastGetField(this Type type, string name)
        {
            return GetField(type, name).Get(null);
        }

        public static TResult FastGetField<TResult>(this Type type, string name)
        {
            return (TResult) type.FastGetField(name);
        }

        public static object FastGetField(this object target, string name)
        {
            if (target == null) throw new ArgumentNullException("target");
            return GetField(target.GetType(), name).Get(target);
        }

        public static TResult FastGetField<TResult>(this object target, string name)
        {
            return (TResult) target.FastGetField(name);
        }

        public static object FastGetField<T>(this T target, string name)
        {
            return GetField<T>(name).Get(target);
        }

        public static TResult FastGetField<T, TResult>(this T target, string name)
        {
            return GetField<T, TResult>(name).Get(target);
        }

        public static void FastSetField<T, TResult>(this T target, string name, TResult value)
        {
            GetField<T, TResult>(name).Set(target, value);
        }

        public static void FastSetField<T>(this T target, string name, object value)
        {
            GetField<T>(name).Set(target, value);
        }

        public static void FastSetField(this object target, string name, object value)
        {
            if (target == null) throw new ArgumentNullException("target");
            GetField(target.GetType(), name).Set(target, value);
        }

        public static void FastSetField<TResult>(this object target, string name, TResult value)
        {
            target.FastSetField(name, (object) value);
        }

        public static void FastSetField(this Type type, string name, object value)
        {
            GetField(type, name).Set(null, value);
        }

        public static void FastSetField<TResult>(this Type type, string name, TResult value)
        {
            type.FastSetField(name, (object) value);
        }

        #endregion

        #region Fast Event

        private static FastEvent GetEvent(Type type, string name)
        {
            FastEvent evt = FastEvent.Get(type, name);
            if (evt == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified event '{0}' in the type {1}.", name,
                    type));
            }
            return evt;
        }

        private static FastEvent<T> GetEvent<T>(string name)
        {
            FastEvent<T> evt = FastEvent<T>.Get(name);
            if (evt == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified event '{0}' in the type {1}.", name,
                    typeof (T)));
            }
            return evt;
        }

        private static FastEvent<T, THandler> GetEvent<T, THandler>(string name)
        {
            FastEvent<T, THandler> evt = FastEvent<T, THandler>.Get(name);
            if (evt == null)
            {
                throw new ArgumentException(string.Format("Cannot find the specified event '{0}' in the type {1}.", name,
                    typeof (T)));
            }
            return evt;
        }

        public static void FastAddEvent<T, THandler>(this T target, string name, THandler handler)
        {
            GetEvent<T, THandler>(name).Add(target, handler);
        }

        public static void FastAddEvent<T>(this T target, string name, Delegate handler)
        {
            GetEvent<T>(name).Add(target, handler);
        }

        public static void FastAddEvent(this object target, string name, Delegate handler)
        {
            if (target == null) throw new ArgumentNullException("target");
            GetEvent(target.GetType(), name).Add(target, handler);
        }

        public static void FastAddEvent(this Type type, string name, Delegate handler)
        {
            GetEvent(type, name).Add(null, handler);
        }

        public static void FastAddEvent<THandler>(this Type type, string name, THandler handler)
        {
            GetEvent(type, name).Add(null, handler as Delegate);
        }

        public static void FastRemoveEvent<T, THandler>(this T target, string name, THandler handler)
        {
            GetEvent<T, THandler>(name).Remove(target, handler);
        }

        public static void FastRemoveEvent<T>(this T target, string name, Delegate handler)
        {
            GetEvent<T>(name).Remove(target, handler);
        }

        public static void FastRemoveEvent(this object target, string name, Delegate handler)
        {
            if (target == null) throw new ArgumentNullException("target");
            GetEvent(target.GetType(), name).Remove(target, handler);
        }

        public static void FastRemoveEvent(this Type type, string name, Delegate handler)
        {
            GetEvent(type, name).Remove(null, handler);
        }

        public static void FastRemoveEvent<THandler>(this Type type, string name, THandler handler)
        {
            GetEvent(type, name).Remove(null, handler as Delegate);
        }

        #endregion
    }
}