
using System.Collections.Generic;
namespace Ucoin.Framework
{
    public class UcoinValidationException  : UcoinException
    {
        #region Properties

        IEnumerable<string> _validationErrors;
        /// <summary>
        /// Get or set the validation errors messages
        /// </summary>
        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Create new instance of Application validation errors exception
        /// </summary>
        /// <param name="validationErrors">The collection of validation errors</param>
        public UcoinValidationException(IEnumerable<string> validationErrors)
            : base("Invalid type,XXXXX")
        {
            _validationErrors = validationErrors;
        }

        #endregion
    }
}
