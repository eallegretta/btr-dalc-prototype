using System;
using System.Linq.Expressions;

namespace Web.Backend.Data.Orm.EntityFramework.Mappings
{
    internal static class ExpressionHelper
    {

        public static Expression<Func<TEntity, TResult>> GetPropertyAccessor<TEntity, TResult>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TEntity), "p");
            var member = Expression.Property(param, propertyName);

            var expression = Expression.Lambda<Func<TEntity, TResult>>(member, param);
            return expression;
        }

    }
}