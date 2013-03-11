using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Backend.Contracts;
using NHibernate.Linq;

namespace Web.Backend.Nhibernate
{
    public class NhibernateQueryableEagerLoadProvider: IQueryableEagerLoadProvider
    {

        public IQueryable<T> DoEagerLoad<T, TRelated>(IQueryable<T> queryable, System.Linq.Expressions.Expression<Func<T, TRelated>> relatedSelector)
            where T : class, new()
            where TRelated : class, new()
        {
            return queryable.Fetch(relatedSelector);
        }

        public IQueryable<T> DoEagerLoadMany<T, TRelated>(IQueryable<T> queryable, System.Linq.Expressions.Expression<Func<T, IEnumerable<TRelated>>> relatedSelector)
            where T : class, new()
            where TRelated : class, new()
        {
            return queryable.FetchMany(relatedSelector);
        }
    }
}