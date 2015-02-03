using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucoin.Framework.Result
{
    public enum ResultStatusType : byte
    {
        /// <summary>
        /// 失败的结果。
        /// </summary>
        Faliure = 0,

        /// <summary>
        /// 成功的结果。
        /// </summary>
        Success = 1
    }
}
