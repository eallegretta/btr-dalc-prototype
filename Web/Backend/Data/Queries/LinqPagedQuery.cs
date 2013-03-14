using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Backend.Data.Queries
{
    public class LinqPagedQuery<T>: LinqQuery<T> where T: class, new()
    {
        private readonly int _skip;
        private readonly int _take;

        public LinqPagedQuery(int skip = 0, int take = 1000)
        {
            _skip = skip;
            _take = take;
        }

        public override void Apply(IQueryable<T> queryable)
        {
            if (_skip > 0)
            {
                queryable = queryable.Skip(_skip);
            }

            queryable = queryable.Take(_take);
        }
    }
}