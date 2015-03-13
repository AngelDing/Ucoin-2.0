using System.Collections.Generic;

namespace Ucoin.Framework
{
    public class UcoinValidationException  : UcoinException
    {
        private IEnumerable<string> validationErrors;

        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return validationErrors;
            }
        }

        public UcoinValidationException(IEnumerable<string> validationErrors)
            : base(GetErrorMessage(validationErrors))
        {
            this.validationErrors = validationErrors;
        }

        private static string GetErrorMessage(IEnumerable<string> validationErrors)
        {
            var msg = string.Empty;
            foreach (var error in validationErrors)
            {
                msg = msg + error + ";";
            }
            return msg;
        }
    }
}
