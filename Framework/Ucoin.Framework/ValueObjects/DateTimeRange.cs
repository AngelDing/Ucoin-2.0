using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ucoin.Framework.ValueObjects
{
    public class DateTimeRange : BaseValueObject
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "Start Date")]
        public DateTime StartDateTime { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "End Date")]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// 用於EF的數據構造
        /// </summary>
        public DateTimeRange()
        {
        }

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
