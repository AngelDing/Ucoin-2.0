using System;
using System.Collections.Generic;

namespace Ucoin.Framework.Logging.Configuration
{
    public class NameValueCollection : Dictionary<string, string>
    {
        public NameValueCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public string GetValue(string key, string defaultValue = null)
        {
            string value;
            if (this.TryGetValue(key, out value) && value != null)
            {
                return value ;
            }
            return defaultValue;
        }

        public new string this[string key]
        {
            get
            {
                string value;
                if (base.TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                base[key] = value;
            }
        }
    }
}
