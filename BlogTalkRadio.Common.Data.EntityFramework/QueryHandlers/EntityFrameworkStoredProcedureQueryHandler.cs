using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using BlogTalkRadio.Common.Data.EntityFramework;
using BlogTalkRadio.Common.Data.FluentMapping;
using BlogTalkRadio.Common.Data.Queries;

namespace BlogTalkRadio.Common.Data.Orm.EntityFramework.QueryHandlers
{
    public class EntityFrameworkStoredProcedureQueryHandler<T> : IQueryHandler<T> where T : class, new()
    {
        private readonly IDbContextFactorySelector _dbContextFactorySelector;

        public EntityFrameworkStoredProcedureQueryHandler(IDbContextFactorySelector dbContextFactorySelector)
        {
            _dbContextFactorySelector = dbContextFactorySelector;
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

        private object ExecuteSpQuery(bool count, StoredProcedureQuery<T> spQuery)
        {
            int parametersCount = spQuery.Parameters != null ? spQuery.Parameters.Count : 0;

            var parameters = new DbParameter[parametersCount];

            var dbContext = _dbContextFactorySelector.GetDbContextFactoryFor(DataSourceMapper.GetDefaultDataSourceForQuery(spQuery)).GetCurrentDbContext();

            using (var cmd = dbContext.Database.Connection.CreateCommand())
            {
                int index = 0;
                if (parametersCount > 0)
                {
                    foreach (var spParam in spQuery.Parameters)
                    {
                        var parameter = cmd.CreateParameter();

                        parameter.ParameterName = spParam.Key;
                        parameter.Value = spParam.Value;

                        parameters[index++] = parameter;
                    }
                }
            }

            if (count)
            {
                using (var cn = dbContext.Database.Connection)
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

            return dbContext.Set<T>().SqlQuery(spQuery.StoredProcedure, parameters).ToList();
        }
    }
}