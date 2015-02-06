using System.ComponentModel;

namespace Ucoin.Framework.Log
{
    public enum LogCategoryType
    {
        [Description("AppLog")]
        AppLog = 1,

        [Description("ErrorLog")]
        ErrorLog = 2,

        [Description("PerfLog")]
        PerfLog = 3
    }
}
