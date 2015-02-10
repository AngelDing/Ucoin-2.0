
using System;
namespace Ucoin.Framework.Utility
{
    public static class StringHelper
    {
        #region To Enum

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToEnum<T>(defaultValue, true);
        }

        public static T ToEnum<T>(this string value, T defaultValue, bool ignoreCase) where T : struct, IComparable, IConvertible, IFormattable
        {
            T o;
            bool flag = Enum.TryParse<T>(value, ignoreCase, out o);
            if (flag && Enum.IsDefined(typeof(T), o))
            {
                return o;
            }
            else
            {
                return defaultValue;
            }
        }

        #endregion

        #region To Bool
        public static bool ToBool(this string str, bool defaultValue)
        {
            bool b;
            if (bool.TryParse(str, out b))
            {
                return b;
            }
            else
            {
                return defaultValue;
            }
        }

        #endregion
    }
}
