using System;
using System.Linq.Expressions;
using Web.Backend.Contracts;

namespace Web.Backend.DomainModel.Queries
{
    public static class Id
    {
        public static IQuery<T> For<T>(string primaryKey, object value)
        {
            return For(PropertyExpression.For<T>(primaryKey), value);
        }

        public static IQuery<T> For<T>(Expression<Func<T, object>> idProperty, object value)
        {
            var equalExpr = BinaryExpression.Equal(idProperty.Body, Expression.Convert(Expression.Constant(value), typeof(object)));
            var lambda = Expression.Lambda<Func<T, bool>>(equalExpr, idProperty.Parameters);
            return new AdHocQuery<T>(lambda);
        }
    }
}
