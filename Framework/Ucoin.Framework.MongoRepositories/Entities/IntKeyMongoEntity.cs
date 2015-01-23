

namespace Ucoin.Framework.Entities
{
    public class IntKeyMongoEntity : BaseMongoEntity, IAggregateRoot<int>
    {
        public int Id { get; set; }
    }
}
