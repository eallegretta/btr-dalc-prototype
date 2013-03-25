using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Web.Backend.Data.Queries;

namespace Web.Backend.Data.Orm.Nhibernate.QueryHandlers
{
    public class NhibernateLinqQueryHandler<T> : IQueryHandler<T> where T : class, new()
    {
        private readonly ISessionFactory _sessionFactory;

        public NhibernateLinqQueryHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is LinqQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            return ApplyQuery(query).Count();
        }

        public List<T> Query(IQuery<T> query)
        {
            return ApplyQuery(query).ToList();
        }

        public T Get(IQuery<T> query)
        {
            return ApplyQuery(query).FirstOrDefault();
        }

        private IQueryable<T> ApplyQuery(IQuery<T> query)
        {
            var linqQuery = query as LinqQuery<T>;

            var queryable = _sessionFactory.GetCurrentSession().Query<T>();

            queryable = linqQuery.Apply(queryable);

            return queryable;
        }
    }
}