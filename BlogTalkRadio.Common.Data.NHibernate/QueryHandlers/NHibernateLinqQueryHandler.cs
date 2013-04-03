using System.Collections.Generic;
using System.Linq;
using BlogTalkRadio.Common.Data.Queries;
using NHibernate;
using NHibernate.Linq;

namespace BlogTalkRadio.Common.Data.NHibernate.QueryHandlers
{
    public class NHibernateLinqQueryHandler<T> : BaseQueryHandler<T> where T : class, new()
    {
        private readonly ISessionFactorySelector _sessionFactorySelector;

        public NHibernateLinqQueryHandler(ISessionFactorySelector sessionFactorySelector)
        {
            _sessionFactorySelector = sessionFactorySelector;
        }

        public override bool CanHandle(IQuery<T> query)
        {
            return query is LinqQuery<T>;
        }

        public override int Count(IQuery<T> query = null)
        {
            return ApplyQuery(query).Count();
        }

        public override List<T> Query(IQuery<T> query)
        {
            return ApplyQuery(query).ToList();
        }

        public override T Get(IQuery<T> query)
        {
            return ApplyQuery(query).FirstOrDefault();
        }

        private IQueryable<T> ApplyQuery(IQuery<T> query)
        {
            var session = _sessionFactorySelector.GetSessionFactoryFor(GetDataSourceForQuery(query)).GetCurrentSession();

            var linqQuery = query as LinqQuery<T>;

            var queryable = session.Query<T>();

            queryable = linqQuery.Apply(queryable);

            return queryable;
        }
    }
}