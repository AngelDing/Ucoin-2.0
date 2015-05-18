using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Framework.SqlDb.BlobStorage
{
    public class BlobEntity : EfEntity<string>
    {
        public BlobEntity(string id, string contentType, byte[] blob, string blobString)
        {
            this.Id = id;
            this.ContentType = contentType;
            this.Blob = blob;
            this.BlobString = blobString;
        }

        public string ContentType { get; set; }

        public byte[] Blob { get; set; }

        /// <devdoc>
        /// This property is only populated by the SQL implementation 
        /// if the content type of the saved blob is "text/plain".
        /// </devdoc>
        public string BlobString { get; set; }
    }
}
