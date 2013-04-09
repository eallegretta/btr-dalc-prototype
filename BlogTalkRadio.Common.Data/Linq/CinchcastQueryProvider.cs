using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BlogTalkRadio.Common.Data.Linq
{
    public abstract class CinchcastQueryProvider : IQueryProvider
    {
        IQueryable<T> IQueryProvider.CreateQuery<T>(Expression expression)
        {
            return new CinchcastQueryable<T>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(CinchcastQueryable<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        T IQueryProvider.Execute<T>(Expression expression)
        {
            return (T)this.Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        public abstract string GetQueryText(Expression expression);
        public abstract object Execute(Expression expression);
    }
}
