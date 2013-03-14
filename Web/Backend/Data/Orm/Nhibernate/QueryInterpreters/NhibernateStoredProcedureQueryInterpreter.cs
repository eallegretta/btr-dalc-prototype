using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NHibernate;
using Web.Backend.Data.Orm.EntityFramework;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel.Contracts;
using IQuery = Web.Backend.DomainModel.Contracts.IQuery;

namespace Web.Backend.Data.Orm.Nhibernate.QueryInterpreters
{
    public class NhibernateStoredProcedureQueryInterpreter<T> : IQueryInterpreter<T> where T: class, new()
    {
        private readonly ISessionFactory _sessionFactory;

        public NhibernateStoredProcedureQueryInterpreter(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool CanInterpret(IQuery query)
        {
            return query is StoredProcedureQuery;
        }

        public int Count(IQuery query = null)
        {
            return GetSqlQuery(query as StoredProcedureQuery).UniqueResult<int>();
        }

        public T Get(IQuery query)
        {
            return GetSqlQuery(query as StoredProcedureQuery).UniqueResult<T>();
        }

        public List<T> Query(IQuery query)
        {
            return GetSqlQuery(query as StoredProcedureQuery).List<T>().ToList();
        }

        private ISession Session
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        private ISQLQuery GetSqlQuery(StoredProcedureQuery spQuery)
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