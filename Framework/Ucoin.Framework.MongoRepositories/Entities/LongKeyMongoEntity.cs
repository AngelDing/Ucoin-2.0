
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.MongoDb.Entities
{
    public class LongKeyMongoEntity : BaseMongoEntity, IAggregateRoot<long>
    {
        public long Id { get; set; }
    }
}
