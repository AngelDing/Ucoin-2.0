using Ucoin.Framework.Entities;

namespace Ucoin.Framework.MongoDb.Entities
{
    public class IntKeyMongoEntity : BaseMongoEntity, IAggregateRoot<int>
    {
        public int Id { get; set; }
    }
}
