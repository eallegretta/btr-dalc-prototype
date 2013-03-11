using System;
using System.Linq.Expressions;

namespace Web.Backend.Contracts
{
    public interface IQuery<T>
    {
        Expression<Func<T, bool>> MatchingCriteria { get; }
    }
}
