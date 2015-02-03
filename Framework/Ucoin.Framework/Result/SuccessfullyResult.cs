using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ucoin.Framework.Result
{
    public class SuccessfullyResult : BaseResult
    {
        static readonly Exception ReadOnlyException = new MemberAccessException(
            "Ucoin.Framework.Result.Successfully 成功结果不允许修改其状态。");

        public override string Message { get { return null; } set { throw ReadOnlyException; } }

        public override bool IsFailed { get { return false; } }

        public override bool IsSucceed { get { return true; } }

        public override ResultStatusType Status { get { return ResultStatusType.Success; } }
    }
}
