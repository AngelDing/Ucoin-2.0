using System;

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

        public override void Validate()
        {
            if (StartDateTime > EndDateTime)
            {
                base.AddBrokenRule(DateTimeRangeBusinessRules.DateTimeError);
            }
        }
    }

    public class DateTimeRangeBusinessRules
    {
        public static readonly BusinessRule DateTimeError =
            new BusinessRule("StartDateTime", "StartDateTime must be before EndDateTime.");        
    }
}
