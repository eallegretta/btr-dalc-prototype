using System.Collections.Generic;
using System.Linq;
using BlogTalkRadio.Common.Data.FluentMapping;

namespace BlogTalkRadio.Common.Data
{
    public abstract class BaseQueryHandler<T> : IQueryHandler<T> where T : class, new()
    {
        protected DataSource GetDataSourceForQuery(IQuery<T> query)
        {
            var dataSource = query.DataSource;

            if (dataSource == null)
            {
                dataSource = DataSourceMapper.GetDefaultReadingDataSourceForQuery(query) ??
                             DataSourceMapper.GetDefaultWritingDataSourceForQuery(query);
            }

            if (dataSource == null)
            {
                dataSource = DataSourceMapper.GetDataSourcesForType<T>().First();
            }

            return dataSource;
        }

        public abstract bool CanHandle(IQuery<T> query);
        public abstract int Count(IQuery<T> query = null);
        public abstract T Get(IQuery<T> query);
        public abstract List<T> Query(IQuery<T> query);
    }
}
