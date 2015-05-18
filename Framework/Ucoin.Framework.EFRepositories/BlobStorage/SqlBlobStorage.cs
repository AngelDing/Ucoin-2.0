using Ucoin.Framework.BlobStorage;

namespace Ucoin.Framework.SqlDb.BlobStorage
{
    public class SqlBlobStorage : IBlobStorage
    {
        public byte[] Find(string id)
        {
            using (var context = new BlobStorageDbContext())
            {
                return context.Find(id);
            }
        }

        public void Save(string id, string contentType, byte[] blob)
        {
            using (var context = new BlobStorageDbContext())
            {
                context.Save(id, contentType, blob);
            }
        }

        public void Delete(string id)
        {
            using (var context = new BlobStorageDbContext())
            {
                context.Delete(id);
            }
        }
    }
}