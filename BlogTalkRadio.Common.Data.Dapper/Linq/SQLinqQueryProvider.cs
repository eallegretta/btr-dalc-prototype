using System;
using SQLinq;
using System.Linq.Expressions;
using BlogTalkRadio.Common.Data.Linq;

namespace BlogTalkRadio.Common.Data.Dapper.Linq
{
    public class SQLinqQueryProvider<T> : CinchcastQueryProvider
    {
        private readonly SQLinq<T> _linq = new SQLinq<T>();

        public override string GetQueryText(Expression expression)
        {
            return _linq.ToSQL().ToQuery();
        }

        public override object Execute(Expression expression)
        {
            var methodExpression = expression as MethodCallExpression;

            ApplyExpression(expression);

            return null;
        }

        private void ApplyExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Call)
            {
                var methodExpression = expression as MethodCallExpression;
                var methodName = methodExpression.Method.Name;

                var expressionValue = (LambdaExpression)((UnaryExpression)methodExpression.Arguments[1]).Operand;

                switch (methodName)
                {
                    case "Select":
                        _linq.Select(Expression.Lambda<Func<T, object>>(expressionValue.Body, expressionValue.Parameters));
                        break;
                    case "Where":
                        _linq.Where(expressionValue);
                        break;
                    case "OrderBy":
                        _linq.OrderBy(Expression.Lambda<Func<T, object>>(expressionValue.Body, expressionValue.Parameters));
                        break;
                    case "OrderByDescending":
                        _linq.OrderByDescending(Expression.Lambda<Func<T, object>>(expressionValue.Body, expressionValue.Parameters));
                        break;
                    case "Skip":
                        break;
                    case "Take":
                        break;
                }

                ApplyExpression(methodExpression.Arguments[0]);
            }
        }
    }
}
