using System;
using System.Linq.Expressions;
using Web.Backend.Contracts;

namespace Web.Backend.DomainModel.Queries
{
    public class AdHocQuery<T>: IQuery<T>
    {
        public AdHocQuery(Expression<Func<T, bool>> matchingCriteria)
        {
            MatchingCriteria = matchingCriteria;
        }

        public Expression<Func<T, bool>> MatchingCriteria { get; private set; }
    }
}
