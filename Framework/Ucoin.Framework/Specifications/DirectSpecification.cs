﻿using System;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Specifications
{
    public sealed class DirectSpecification<T> : Specification<T> where T : IEntity
    {
        private Expression<Func<T, bool>> matchingCriteria;

        public DirectSpecification(Expression<Func<T, bool>> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }
            matchingCriteria = criteria;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return matchingCriteria;
        }
    }
}
