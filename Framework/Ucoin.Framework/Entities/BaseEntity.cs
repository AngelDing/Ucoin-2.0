using System;
namespace Ucoin.Framework.Entities
{
    [Serializable]
    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        [CompareKey]
        public TKey Id { get; set; }

        public override bool Equals(object entity)
        {
            return entity != null
               && entity is BaseEntity<TKey>
               && this == (BaseEntity<TKey>)entity;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            if (entity1.Id.ToString() == entity2.Id.ToString())
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            return (!(entity1 == entity2));
        }
    }
}
