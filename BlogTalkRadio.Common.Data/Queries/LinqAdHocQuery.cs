using System;
using System.Linq;
using System.Linq.Expressions;

namespace BlogTalkRadio.Common.Data.Queries
{
    public class LinqAdHocQuery<T> : LinqQuery<T> where T : class, new()
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        private readonly Expression<Func<T, bool>> _filterExpr;

        public LinqAdHocQuery(Expression<Func<T, bool>>  filterExpr = null)
        {
            _filterExpr = filterExpr;
        }

        public override IQueryable<T> Apply(IQueryable<T> queryable)
        {
            if (_filterExpr != null)
            {
                queryable = queryable.Where(_filterExpr);
            }

            if (Skip.HasValue && Skip.Value > 0)
            {
                queryable = queryable.Skip(Skip.Value);
            }

            if (Take.HasValue)
            {
                queryable = queryable.Take(Take.Value);
            }

            return queryable;
        }
    }
}