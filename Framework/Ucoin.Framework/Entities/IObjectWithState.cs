namespace Ucoin.Framework.Entities
{
    public interface IObjectWithState
    {
        [CompareIgnore]
        ObjectStateType ObjectState { get; set; }
    }

    public enum ObjectStateType
    {
        Added,
        Unchanged,
        Modified,
        Deleted
    }
}
