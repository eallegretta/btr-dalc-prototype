using Autofac;
using Autofac.Core;
using BlogTalkRadio.Common.Data.DataSources;
using Cinchcast.Framework.DependencyInjection.Autofac;
using NHibernate;
namespace BlogTalkRadio.Common.Data.NHibernate
{
    public interface ISessionFactorySelector
    {
        ISessionFactory GetSessionFactoryFor(DataSource dataSource);
    }

    public class SessionFactorySelector: ISessionFactorySelector
    {
        public SessionFactorySelector()
        {
        }

        public ISessionFactory GetSessionFactoryFor(DataSource dataSource)
        {
            var sqlDataSource = dataSource as SqlDataSource;

            return Ioc.Instance.Container.ResolveNamed<ISessionFactory>(sqlDataSource.ConnectionString.Name);
        }
    }
}
