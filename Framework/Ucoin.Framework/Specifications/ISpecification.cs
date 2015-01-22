using System;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Specifications
{
    public interface ISpecification<T> where T : IEntity
    {
        Expression<Func<T, bool>> SatisfiedBy();
    }
}
