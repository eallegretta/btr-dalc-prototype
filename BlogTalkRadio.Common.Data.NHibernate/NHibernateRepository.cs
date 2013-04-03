using System.Collections.Generic;
using System.Linq;
using BlogTalkRadio.Common.Data.FluentMapping;
using NHibernate;

namespace BlogTalkRadio.Common.Data.NHibernate
{
    public class NHibernateRepository<T> : BaseRepository<T> where T : class, new()
    {
        private readonly ISessionFactorySelector _sessionFactorySelector;

        public NHibernateRepository(ISessionFactorySelector sessionFactorySelector, IEnumerable<IQueryHandler<T>> queryHandlers)
            : base(queryHandlers)
        {
            _sessionFactorySelector = sessionFactorySelector;
        }

        private ISession Session(bool forReading = true)
        {
            var dataSource = forReading
                             ? DataSourceMapper.GetDefaultReadingDataSourceForType<T>()
                             : DataSourceMapper.GetDefaultWritingDataSourceForType<T>();

            if (dataSource == null)
            {
                dataSource = DataSourceMapper.GetDataSourcesForType<T>().First();
            }
        
            return _sessionFactorySelector.GetSessionFactoryFor(dataSource).GetCurrentSession();
        }

        public override T Get(object id)
        {
            return Session().Get<T>(id);
        }

        public override T SaveOrUpdate(T instance)
        {
            Session(false).SaveOrUpdate(instance);
            return instance;
        }

        public override void Delete(object id)
        {
            var session = Session(false);

            session.Delete(id);
            session.Flush();
        }

        public override void DeleteAll()
        {
            var session = Session(false);

            session.Delete("from " + typeof(T).Name + " e");
            session.Flush();
        }
    }
}
