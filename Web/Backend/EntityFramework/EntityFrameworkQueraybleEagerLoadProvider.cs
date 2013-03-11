﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web.Backend.Contracts;

namespace Web.Backend.EntityFramework
{
    public class EntityFrameworkQueraybleEagerLoadProvider : IQueryableEagerLoadProvider
    {
        public IQueryable<T> DoEagerLoad<T, TRelated>(IQueryable<T> queryable, System.Linq.Expressions.Expression<Func<T, TRelated>> relatedSelector)
            where T : class, new()
            where TRelated : class, new()
        {
            return queryable.Include(relatedSelector);
        }

        public IQueryable<T> DoEagerLoadMany<T, TRelated>(IQueryable<T> queryable, System.Linq.Expressions.Expression<Func<T, IEnumerable<TRelated>>> relatedSelector)
            where T : class, new()
            where TRelated : class, new()
        {
            return queryable.Include(relatedSelector);
        }
    }
}