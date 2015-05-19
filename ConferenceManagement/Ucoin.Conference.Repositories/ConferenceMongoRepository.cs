using System.Configuration;
using Ucoin.Framework.MongoDb.Entities;
using Ucoin.Framework.MongoDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public class ConferenceMongoRepository<T> : MongoRepository<T, string> where T : StringKeyMongoEntity
    {
        public ConferenceMongoRepository()
            : base(ConfigurationManager.ConnectionStrings["ConferenceMongoDb"].ConnectionString)
        {
        }
    }
}
