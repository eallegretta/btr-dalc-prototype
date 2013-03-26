using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Web.Backend.Data.Queries;

namespace Web.Backend.Data.Orm.EntityFramework.QueryHandlers
{
    public class EntityFrameworkStoredProcedureQueryHandler<T> : IQueryHandler<T> where T: class, new()
    {
        private readonly IDbContextFactory _dbContextFactory;

        public EntityFrameworkStoredProcedureQueryHandler(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is StoredProcedureQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            return (int)ExecuteSpQuery(false, query as StoredProcedureQuery<T>);
        }

        public T Get(IQuery<T> query)
        {
            return ((List<T>)ExecuteSpQuery(false, query as StoredProcedureQuery<T>)).FirstOrDefault();
        }

        public List<T> Query(IQuery<T> query)
        {
            return (List<T>)ExecuteSpQuery(false, query as StoredProcedureQuery<T>);
        }

        private DbContext DbContext
        {
            get { return _dbContextFactory.GetCurrentDbContext(); }
        }

        private object ExecuteSpQuery(bool count, StoredProcedureQuery<T> spQuery)
        {
            var parameters = new DbParameter[spQuery.Parameters.Count];

            using (var cmd = DbContext.Database.Connection.CreateCommand())
            {
                int index = 0;
                foreach (var spParam in spQuery.Parameters)
                {
                    var parameter = cmd.CreateParameter();

                    parameter.ParameterName = spParam.Key;
                    parameter.Value = spParam.Value;

                    parameters[index++] = parameter;
                }
            }

            if (count)
            {
                using (var cn = DbContext.Database.Connection)
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    }

                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = spQuery.StoredProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }

                        return cmd.ExecuteScalar();
                    }
                }
            }

            return  DbContext.Set<T>().SqlQuery(spQuery.StoredProcedure, parameters).ToList();
        }
    }
}