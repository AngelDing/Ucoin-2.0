using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class ResourceColumn : EfEntity<int>
    {
        public int ResourceId { get; set; }

        public bool IsReject { get; set; }

        public string ColumnName { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
