
using Ucoin.Framework.Entities;
namespace Ucoin.Framework.Specifications
{
    public abstract class CompositeSpecification<T> : Specification<T> where T : IEntity
    {
        public abstract ISpecification<T> LeftSpec { get; }

        public abstract ISpecification<T> RightSpec { get; }
    }
}
