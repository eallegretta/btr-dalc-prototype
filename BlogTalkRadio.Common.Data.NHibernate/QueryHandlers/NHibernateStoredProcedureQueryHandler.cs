using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogTalkRadio.Common.Data.Queries;
using NHibernate;

namespace BlogTalkRadio.Common.Data.NHibernate.QueryHandlers
{
    public class NHibernateStoredProcedureQueryHandler<T> : BaseQueryHandler<T> where T: class, new()
    {
        private readonly ISessionFactorySelector _sessionFactorySelector;

        public NHibernateStoredProcedureQueryHandler(ISessionFactorySelector sessionFactorySelector)
        {
            _sessionFactorySelector = sessionFactorySelector;
        }

        public override bool CanHandle(IQuery<T> query)
        {
            return query is StoredProcedureQuery<T>;
        }

        public override int Count(IQuery<T> query = null)
        {
            return GetSqlQuery(query as StoredProcedureQuery<T>).UniqueResult<int>();
        }

        public override T Get(IQuery<T> query)
        {
            return GetSqlQuery(query as StoredProcedureQuery<T>).UniqueResult<T>();
        }

        public override List<T> Query(IQuery<T> query)
        {
            return GetSqlQuery(query as StoredProcedureQuery<T>).List<T>().ToList();
        }

        private ISQLQuery GetSqlQuery(StoredProcedureQuery<T> spQuery)
        {
            var session = _sessionFactorySelector.GetSessionFactoryFor(GetDataSourceForQuery(spQuery)).GetCurrentSession();

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

            return query;
        }
    }
}