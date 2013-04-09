using System.Collections.Generic;
using System.Linq;
using BlogTalkRadio.Common.Data.FluentMapping;
using BlogTalkRadio.Common.Data.Queries;
using NHibernate;
using NHibernate.Linq;

namespace BlogTalkRadio.Common.Data.NHibernate.QueryHandlers
{
    public class NHibernateLinqQueryHandler<T> : IQueryHandler<T> where T : class, new()
    {
        private readonly ISessionFactorySelector _sessionFactorySelector;

        public NHibernateLinqQueryHandler(ISessionFactorySelector sessionFactorySelector)
        {
            _sessionFactorySelector = sessionFactorySelector;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is LinqQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            var queryable = ApplyQuery(query);

            return queryable.Skip(0).Take(1).Count();
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
            var session = _sessionFactorySelector.GetSessionFactoryFor(DataSourceMapper.GetDefaultDataSourceForQuery(query)).GetCurrentSession();

            var linqQuery = query as LinqQuery<T>;

            var queryable = session.Query<T>();

            queryable = linqQuery.Apply(queryable);

            return queryable;
        }
    }
}