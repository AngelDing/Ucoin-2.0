using System.ComponentModel;

namespace Ucoin.Framework
{
    public enum AppCodeType : byte
    {
        [Description("System")]
        System = 1,

        [Description("Common")]
        Common = 2,

        [Description("Authority")]
        Authority = 3,
    }
}
