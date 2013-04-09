using Autofac;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.Orm.EntityFramework;
using Cinchcast.Framework.DependencyInjection.Autofac;
using Web.Backend.DependencyInjection.EntityFramework;

namespace BlogTalkRadio.Common.Data.EntityFramework
{
    public interface IDbContextFactorySelector
    {
        IDbContextFactory GetDbContextFactoryFor(DataSource dataSource);
    }

    public class DbContextFactorySelector : IDbContextFactorySelector
    {
        public IDbContextFactory GetDbContextFactoryFor(DataSource dataSource)
        {
            var sqlDataSource = dataSource as SqlDataSource;

            return Ioc.Instance.Container.ResolveNamed<IDbContextFactory>(string.Format(EntityFrameworkModule.DBCONTEXT_FACTORY_KEY, sqlDataSource.ConnectionString.Name));
        }
    }
}
