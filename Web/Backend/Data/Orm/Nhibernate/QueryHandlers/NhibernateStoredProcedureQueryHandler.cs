using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Web.Backend.Data.Queries;

namespace Web.Backend.Data.Orm.Nhibernate.QueryHandlers
{
    public class NhibernateStoredProcedureQueryHandler<T> : IQueryHandler<T> where T: class, new()
    {
        private readonly ISessionFactory _sessionFactory;

        public NhibernateStoredProcedureQueryHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
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

        private ISession Session
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        private ISQLQuery GetSqlQuery(StoredProcedureQuery<T> spQuery)
        {
            var sql = new StringBuilder();
            sql.Append("exec ");
            sql.Append(spQuery.StoredProcedure);

            foreach (var param in spQuery.Parameters.Keys)
            {
                sql.AppendFormat(" @{0}=:{0}", param);
            }

            var query = Session.CreateSQLQuery(sql.ToString());

            foreach (var param in spQuery.Parameters)
            {
                query.SetParameter(param.Key, param.Value);
            }

            return query;
        }
    }
}