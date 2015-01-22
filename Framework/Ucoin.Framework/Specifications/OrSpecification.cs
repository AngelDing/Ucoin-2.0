using System;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Specifications
{
    public sealed class OrSpecification<T> : CompositeSpecification<T> where T : IEntity
    {
        private ISpecification<T> rightSpec = null;
        private ISpecification<T> leftSpec = null;

        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == null)
            {
                throw new ArgumentNullException("leftSide");
            }
            if (rightSide == null)
            {
                throw new ArgumentNullException("rightSide");
            }
            this.leftSpec = leftSide;
            this.rightSpec = rightSide;
        }

        public override ISpecification<T> LeftSpec
        {
            get { return leftSpec; }
        }

        public override ISpecification<T> RightSpec
        {
            get { return rightSpec; }
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = leftSpec.SatisfiedBy();
            Expression<Func<T, bool>> right = rightSpec.SatisfiedBy();
            return left.Or(right);
        }
    }
}
