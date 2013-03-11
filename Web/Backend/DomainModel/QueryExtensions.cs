using System.Linq;
using Web.Backend.Contracts;

namespace Web.Backend.DomainModel
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, IQuery<T> query)
        {
            if (query == null) return queryable;

            return queryable.Where(query.MatchingCriteria);
        }
    }
}