
using MongoDB.Bson;

namespace Ucoin.Framework.MongoDb.Managers
{
    public class FieldModel : BaseModel, IModel
    {
        public CollectionModel Collection { get; set; }

        public BsonType Type { get; set; }
    }
}
