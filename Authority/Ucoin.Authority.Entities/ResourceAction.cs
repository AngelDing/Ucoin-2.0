using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class ResourceAction : EfEntity<int>
    {
        public int ResourceId { get; set; }

        public int ActionId { get; set; }

        public string Url { get; set; }

        public bool AccessControl { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual Action Action { get; set; }
    }
}
