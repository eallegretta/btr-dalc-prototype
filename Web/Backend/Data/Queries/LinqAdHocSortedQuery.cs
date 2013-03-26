using System;
using System.Linq;
using System.Linq.Expressions;

namespace Web.Backend.Data.Queries
{
    public class LinqAdHocSortedQuery<T, TKey> : LinqAdHocQuery<T> where T : class, new()
    {
        private readonly Expression<Func<T, TKey>> _sortExpr;
        private readonly bool _descending;

        public LinqAdHocSortedQuery(Expression<Func<T, bool>> filterExpr = null, Expression<Func<T, TKey>> sortExpr = null, bool descending = false): base(filterExpr)
        {
            _sortExpr = sortExpr;
            _descending = @descending;
        }

        public override IQueryable<T> Apply(IQueryable<T> queryable)
        {
            if (_sortExpr != null)
            {
                queryable = _descending ? queryable.OrderByDescending(_sortExpr) : queryable.OrderBy(_sortExpr);
            }

            return base.Apply(queryable);
        }
    }
}