using System;
using Autofac;
using Autofac.Core;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.NHibernate.DependencyInjection;
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

            if (sqlDataSource == null)
            {
                throw new Exception("Only SqlDataSource is supported");
            }

            return Ioc.Instance.Container.ResolveNamed<ISessionFactory>(string.Format(NHibernateModule.SESSION_FACTORY_KEY, sqlDataSource.ConnectionString.Name));
        }
    }
}
