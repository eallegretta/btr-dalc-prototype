using System;
using System.Linq.Expressions;

namespace BlogTalkRadio.Common.Data
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