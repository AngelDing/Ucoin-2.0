using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ucoin.Framework.ValueObjects
{
    public class DateTimeRange : BaseValueObject
    {
        public DateTime StartDateTime { get; private set; }

        public DateTime EndDateTime { get; private set; }

        public DateTimeRange(DateTime start, DateTime end)
        {
            StartDateTime = start;
            EndDateTime = end;

            base.ThrowExceptionIfInvalid();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            if (StartDateTime > EndDateTime)
            {
                //TODO: Message可放入資源文件中維護
                validationResults.Add(new ValidationResult("StartDateTime must be before EndDateTime.",
                                                         new string[] { "StartDateTime" }));
            }

            return validationResults;
        }
    }
}
