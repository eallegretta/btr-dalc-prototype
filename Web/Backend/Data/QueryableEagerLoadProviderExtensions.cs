using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.ServiceLocation;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data
{
    public static class QueryableEagerLoadProviderExtensions
    {
        public static IQueryable<T> EagerLoad<T, TRelated>(this IQueryable<T> queryable,
                                                           Expression<Func<T, TRelated>> relatedSelector)
            where T : class, new()
            where TRelated : class, new()
        {
            return ServiceLocator.Current.GetInstance<IQueryableEagerLoadProvider>().DoEagerLoad(queryable, relatedSelector);
        }

        public static IQueryable<T> EagerLoadMany<T, TRelated>(this IQueryable<T> queryable, Expression<Func<T, IEnumerable<TRelated>>> relatedSelector)
            where T : class, new()
            where TRelated : class, new()
        {
            return ServiceLocator.Current.GetInstance<IQueryableEagerLoadProvider>().DoEagerLoadMany(queryable, relatedSelector);
        }
    }
}