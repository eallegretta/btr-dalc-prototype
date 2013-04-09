using System.Linq;
using BlogTalkRadio.Common.Data.FluentMapping;
using BlogTalkRadio.Common.Data.NHibernate.Queries;
using NHibernate;

namespace BlogTalkRadio.Common.Data.NHibernate.QueryHandlers
{
    public class NHibernateQueryHandler<T> : IQueryHandler<T> where T : class, new()
    {
        private readonly ISessionFactorySelector _sessionFactorySelector;

        public NHibernateQueryHandler(ISessionFactorySelector sessionFactorySelector)
        {
            _sessionFactorySelector = sessionFactorySelector;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is NHibernateQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            return ApplyQuery(query).RowCount();
        }

        public T Get(IQuery<T> query)
        {
            return ApplyQuery(query).SingleOrDefault();
        }

        public System.Collections.Generic.List<T> Query(IQuery<T> query)
        {
            return ApplyQuery(query).List().ToList();
        }

        private IQueryOver<T, T> ApplyQuery(IQuery<T> query)
        {
            var nhQuery = query as NHibernateQuery<T>;

            var session = _sessionFactorySelector.GetSessionFactoryFor(DataSourceMapper.GetDefaultDataSourceForQuery(nhQuery)).GetCurrentSession();

            return nhQuery.CreateQuery(session.QueryOver<T>());
        }
    }
}
