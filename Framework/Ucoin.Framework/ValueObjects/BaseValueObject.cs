using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucoin.Framework.ValueObjects
{
    /// <summary>
    /// DDD中值對象的基類
    /// </summary>
    [Serializable]
    public abstract class BaseValueObject : IValidate
    {
        private List<BusinessRule> brokenRules = new List<BusinessRule>();

        public BaseValueObject()
        {
        }

        public virtual void Validate()
        { 
        }

        public void ThrowExceptionIfInvalid()
        {
            brokenRules.Clear();
            Validate();
            if (brokenRules.Count() > 0)
            {
                var issues = new StringBuilder();
                foreach (BusinessRule businessRule in brokenRules)
                {
                    issues.AppendLine(businessRule.Rule);
                }

                throw new UcoinException(issues.ToString());
            }
        }

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            brokenRules.Add(businessRule);
        }
    }
}
