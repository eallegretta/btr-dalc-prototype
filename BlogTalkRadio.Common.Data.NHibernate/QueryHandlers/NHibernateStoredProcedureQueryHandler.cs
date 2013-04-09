using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogTalkRadio.Common.Data.FluentMapping;
using BlogTalkRadio.Common.Data.Queries;
using NHibernate;
using NHibernate.Transform;

namespace BlogTalkRadio.Common.Data.NHibernate.QueryHandlers
{
    public class NHibernateStoredProcedureQueryHandler<T> : IQueryHandler<T> where T : class, new()
    {
        private readonly ISessionFactorySelector _sessionFactorySelector;

        public NHibernateStoredProcedureQueryHandler(ISessionFactorySelector sessionFactorySelector)
        {
            _sessionFactorySelector = sessionFactorySelector;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is StoredProcedureQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            return GetSqlQuery(query as StoredProcedureQuery<T>).UniqueResult<int>();
        }

        public T Get(IQuery<T> query)
        {
            return GetSqlQuery(query as StoredProcedureQuery<T>).UniqueResult<T>();
        }

        public List<T> Query(IQuery<T> query)
        {
            return GetSqlQuery(query as StoredProcedureQuery<T>).List<T>().ToList();
        }

        private ISQLQuery GetSqlQuery(StoredProcedureQuery<T> spQuery)
        {
            var session = _sessionFactorySelector.GetSessionFactoryFor(DataSourceMapper.GetDefaultDataSourceForQuery(spQuery)).GetCurrentSession();

            var sql = new StringBuilder();
            sql.Append("exec ");
            sql.Append(spQuery.StoredProcedure);

            if (spQuery.Parameters != null)
            {
                foreach (var param in spQuery.Parameters.Keys)
                {
                    sql.AppendFormat(" @{0}=:{0}", param);
                }
            }

            var query = session.CreateSQLQuery(sql.ToString());

            if (spQuery.Parameters != null)
            {
                foreach (var param in spQuery.Parameters)
                {
                    query.SetParameter(param.Key, param.Value);
                }
            }

            query.AddEntity(typeof (T));

            return query;
        }
    }
}