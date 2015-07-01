using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class ResourceButton : EfEntity<int>
    {
        public int ResourceId { get; set; }

        public int ButtonId { get; set; }

        public string Url { get; set; }

        public bool AccessControl { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual Button Button { get; set; }
    }
}
