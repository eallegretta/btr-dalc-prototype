using System;
using System.Configuration;

namespace BlogTalkRadio.Common.Data.DataSources
{
    public class SqlDataSource: DataSource
    {
        public SqlDataSource(string identifier) : base(identifier)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[identifier];

            if (ConnectionString == null)
            {
                throw new Exception("The connection string does not exist");
            }
        }

        public ConnectionStringSettings ConnectionString
        {
            get; private set;
        }
        
    }
}
