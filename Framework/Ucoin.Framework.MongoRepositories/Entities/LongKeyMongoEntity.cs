

namespace Ucoin.Framework.Entities
{
    public class LongKeyMongoEntity : BaseMongoEntity, IAggregateRoot<long>
    {
        public long Id { get; set; }
    }
}
