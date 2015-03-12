using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Ucoin.Framework.ValueObjects
{
    public class Money : BaseValueObject
    {
        public decimal Value { get; private set; }

        public string Currency { get; private set; }

        public Money(decimal amount, string threeLetterISOCode)
        {
            Value = amount;
            Currency = threeLetterISOCode;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            if (Currency != null || Currency.Length != 3)
            {
                //TODO: Message可放入資源文件中維護
                validationResults.Add(new ValidationResult("The currency ISO code must be 3 letters in length.",
                                                         new string[] { "Currency" }));
                //base.AddBrokenRule(MoneyBusinessRules.CurrencyISOCodeError);
            }

            return validationResults;
        }
    }
}
