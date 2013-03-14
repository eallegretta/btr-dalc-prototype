using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Orm.Nhibernate
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