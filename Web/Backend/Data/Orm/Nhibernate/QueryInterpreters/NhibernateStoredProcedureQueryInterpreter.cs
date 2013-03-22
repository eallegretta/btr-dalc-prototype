using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NHibernate;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Orm.Nhibernate.QueryInterpreters
{
    public class NhibernateStoredProcedureQueryInterpreter<T> : IQueryInterpreter<T> where T: class, new()
    {
        private readonly ISessionFactory _sessionFactory;

        public NhibernateStoredProcedureQueryInterpreter(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool CanInterpret(IQuery<T> query)
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