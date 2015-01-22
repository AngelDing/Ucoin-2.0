using System;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Specifications
{
    public abstract class Specification<T> : ISpecification<T> where T : IEntity
    {
        public abstract Expression<Func<T, bool>> SatisfiedBy();

        #region Override Operators

        public static Specification<T> operator &(Specification<T> leftSpec, Specification<T> rightSpec)
        {
            return new AndSpecification<T>(leftSpec, rightSpec);
        }

        public static Specification<T> operator |(Specification<T> leftSpec, Specification<T> rightSpec)
        {
            return new OrSpecification<T>(leftSpec, rightSpec);
        }

        public static Specification<T> operator !(Specification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }

        public static bool operator false(Specification<T> specification)
        {
            return false;
        }

        public static bool operator true(Specification<T> specification)
        {
            return true;
        }

        #endregion
    }
}
