using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Orm.EntityFramework.QueryInterpreters
{
    public class EntityFrameworkStoredProcedureQueryInterpreter<T> : IQueryInterpreter<T> where T: class, new()
    {
        private readonly IDbContextFactory _dbContextFactory;

        public EntityFrameworkStoredProcedureQueryInterpreter(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public bool CanInterpret(IQuery query)
        {
            return query is StoredProcedureQuery;
        }

        public int Count(IQuery query = null)
        {
            return (int)ExecuteSpQuery(false, query as StoredProcedureQuery);
        }

        public T Get(IQuery query)
        {
            return ((List<T>)ExecuteSpQuery(false, query as StoredProcedureQuery)).FirstOrDefault();
        }

        public List<T> Query(IQuery query)
        {
            return (List<T>) ExecuteSpQuery(false, query as StoredProcedureQuery);
        }

        private DbContext DbContext
        {
            get { return _dbContextFactory.GetCurrentDbContext(); }
        }

        private object ExecuteSpQuery(bool count, StoredProcedureQuery spQuery)
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