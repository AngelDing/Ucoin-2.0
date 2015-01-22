namespace Ucoin.Framework.CompareObjects
{
    public interface ICompareLogic
    {
        ComparisonConfig Config { get; set; }

        ComparisonResult Compare(object object1, object object2);
    }
}
