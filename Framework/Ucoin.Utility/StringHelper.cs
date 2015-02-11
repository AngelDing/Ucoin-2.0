
using System;
namespace Ucoin.Framework.Utility
{
    public static class StringHelper
    {
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
