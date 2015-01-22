
using System.Collections.Generic;
namespace Ucoin.Framework.MongoRepository.Manager
{
    public class IndexModel : BaseModel, IModel
    {
        public CollectionModel Collection { get; set; }

        public string Namespace { get; set; }

        public List<IndexKey> Keys { get; set; }

        public bool Unique { get; set; }
    }

    public class IndexKey
    {
        public string FieldName { get; set; }

        public IndexOrderType OrderType { get; set; }
    }
}
