using System;
using System.Linq.Expressions;

namespace BlogTalkRadio.Common.Data.Orm.EntityFramework.Mappings
{
    public static class ExpressionHelper
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