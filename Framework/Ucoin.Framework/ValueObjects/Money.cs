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

        public override void Validate()
        {
            if (Currency != null || Currency.Length != 3)
            {
                base.AddBrokenRule(MoneyBusinessRules.CurrencyISOCodeError);
            }
        }
    }

    public class MoneyBusinessRules
    {
        public static readonly BusinessRule CurrencyISOCodeError =
           new BusinessRule("Currency", "The currency ISO code must be 3 letters in length.");      
    }
}
