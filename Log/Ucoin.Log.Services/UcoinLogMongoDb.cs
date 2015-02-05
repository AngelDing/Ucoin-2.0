using System.Configuration;
using Ucoin.Framework.MongoDb.Entities;
using Ucoin.Framework.MongoDb.Repositories;

namespace Ucoin.Log.Services
{
    public class UcoinLogMongoDb<T> : MongoRepository<T, string> where T : StringKeyMongoEntity
    {
        public UcoinLogMongoDb()
            : base(ConfigurationManager.ConnectionStrings["UcoinLogMongoDb"].ConnectionString)
        {
        }
    }
}
