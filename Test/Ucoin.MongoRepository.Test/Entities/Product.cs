using Ucoin.Framework.Entities;

namespace Ucoin.MongoRepository.Test
{
    public class Product : StringKeyMongoEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
