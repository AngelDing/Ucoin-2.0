using System;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Specifications
{
    public sealed class TrueSpecification<T> : Specification<T> where T : IEntity
    {
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            bool result = true;
            Expression<Func<T, bool>> trueExpression = t => result;
            return trueExpression;
        }
    }
}
