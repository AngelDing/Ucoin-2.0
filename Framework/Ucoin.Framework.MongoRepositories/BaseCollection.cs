namespace Ucoin.Framework.MongoDb
{
    public class BaseCollection<T> : BaseMongoDB<T> where T : class
    {
        public BaseCollection(): base(null)
        { 
        }
    }
}
