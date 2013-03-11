using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Web.Backend.DomainModel
{
    public static class PropertyExpression
    {
        public static Expression<Func<T, object>> For<T>(string propertyName)
        {
            var argParam = Expression.Parameter(typeof(T), "x");
            var idProperty = Expression.Property(argParam, propertyName);
            var convert = Expression.Convert(idProperty, typeof (object));

            var lambda = Expression.Lambda<Func<T, object>>(convert, argParam);
            return lambda;
        }
    }
}