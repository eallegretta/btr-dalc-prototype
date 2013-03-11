﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Web.Backend.Contracts
{
    public interface IQueryableEagerLoadProvider
    {
        IQueryable<T> DoEagerLoad<T, TRelated>(IQueryable<T> queryable, Expression<Func<T, TRelated>> relatedSelector)
            where T : class, new()
            where TRelated : class, new();
        IQueryable<T> DoEagerLoadMany<T, TRelated>(IQueryable<T> queryable, Expression<Func<T, IEnumerable<TRelated>>> relatedSelector)
            where T : class, new()
            where TRelated : class, new();
    }
}