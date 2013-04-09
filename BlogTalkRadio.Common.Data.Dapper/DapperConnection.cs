using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.FluentMapping;

namespace BlogTalkRadio.Common.Data.Dapper
{
    public interface IDapperConnection
    {
        IDbConnection For<T>();
        IDbConnection For<T>(IQuery<T> query) where T : class, new();
    }

    public class DapperConnection : IDapperConnection
    {
        public IDbConnection For<T>()
        {
            return GetConnection(DataSourceMapper.GetDefaultDataSourceForType<T>());
        }

        public IDbConnection For<T>(IQuery<T> query) where T : class, new()
        {
            return GetConnection(DataSourceMapper.GetDefaultDataSourceForQuery(query));
        }

        private static IDbConnection GetConnection(DataSource dataSource)
        {
            var sqlDataSource = dataSource as SqlDataSource;

            if (sqlDataSource == null)
            {
                throw new Exception("Dapper only supports SqlDataSource");
            }

            var dbFactory = DbProviderFactories.GetFactory(sqlDataSource.ConnectionString.ProviderName);

            var cn = dbFactory.CreateConnection();
            cn.ConnectionString = sqlDataSource.ConnectionString.ConnectionString;
            cn.Open();
            return cn;
        }
    }
}
