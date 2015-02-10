using System;

namespace Ucoin.Framework.Utility
{
    public static class ParamterCheckHelper
    {
        public static void CheckNotNullOrEmpty(this string value, string paramName)
        {
            value.CheckNotNull(paramName);
            Require<ArgumentException>(value.Length > 0, string.Format("参数“{0}”不能为空引用或空字符串。", paramName));
        }

        public static void CheckNotNull<T>(this T value, string paramName) where T : class
        {
            Require<ArgumentNullException>(value != null, string.Format("参数“{0}”不能为空引用。", paramName));
        }

        private static void Require<TException>(bool assertion, string message) where TException : Exception
        {
            if (assertion)
            {
                return;
            }
            TException exception = (TException)Activator.CreateInstance(typeof(TException), message);
            throw exception;
        }

        public static void CheckIsAssignable<T>(this Type valType, string paramName, string message)
        {
            valType.CheckNotNull("valType");

            if (!typeof(T).IsAssignableFrom(valType))
            {
                throw new ArgumentOutOfRangeException(paramName, valType, message);
            }
        }
    }
}
