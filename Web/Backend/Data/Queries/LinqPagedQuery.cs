using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Backend.Data.Queries
{
    public class LinqPagedQuery<T>: LinqQuery<T> where T: class, new()
    {
        public LinqPagedQuery()
        {
            Skip = 0;
            Take = 1000;
        }

        public int Skip { get; set; }
        public int Take { get; set; }

        public override IQueryable<T> Apply(IQueryable<T> queryable)
        {
            if (Skip > 0)
            {
                queryable = queryable.Skip(Skip);
            }

            return queryable.Take(Take);
        }
    }
}