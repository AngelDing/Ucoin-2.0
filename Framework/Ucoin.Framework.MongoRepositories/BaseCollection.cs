namespace Ucoin.Framework.MongoRepository
{
    public class BaseCollection<T> : BaseMongoDB<T> where T : class
    {
        public BaseCollection()
        { 
        }
    }
}
