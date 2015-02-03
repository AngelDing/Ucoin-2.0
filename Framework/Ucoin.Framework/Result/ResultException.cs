
using System;
namespace Ucoin.Framework.Result
{
    public class ResultException : Exception
    {
        public ResultException(string message, ResultStatusType status = ResultStatusType.Faliure)
            : base(message)
        {
            if (status == ResultStatusType.Success)
            {
                throw new ArgumentOutOfRangeException("status error");
            }
        }
    }
}
