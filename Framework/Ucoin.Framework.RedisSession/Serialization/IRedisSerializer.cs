namespace Ucoin.Framework.RedisSession
{   
    public interface IRedisSerializer
    {
        object DeserializeOne(string objRaw);

        string SerializeOne(object origObj);
    }
}
