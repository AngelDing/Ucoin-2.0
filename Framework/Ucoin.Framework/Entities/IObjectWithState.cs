namespace Ucoin.Framework.Entities
{
    public interface IObjectWithState
    {
        [CompareIgnore]
        ObjectStateType State { get; set; }
    }

    public enum ObjectStateType
    {
        Added,
        Unchanged,
        Modified,
        Deleted
    }
}
