using Autofac;
using Cinchcast.Framework.DependencyInjection.Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Bytecode;
using Web.Backend.Data;
using Web.Backend.Data.Orm.Nhibernate;
using Web.Backend.Data.Orm.Nhibernate.QueryHandlers;
using Web.Backend.DomainModel.Contracts;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.DependencyInjection.Nhibernate
{
    public class NhibernateModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => Initialize("Default"))
                   .As<ISessionFactory>()
                   .SingleInstance();

            builder.RegisterType<NhibernateUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<NhibernateQueryableEagerLoadProvider>().As<IQueryableEagerLoadProvider>();
            builder.RegisterGeneric(typeof (NhibernateRepository<>)).As(typeof (IRepository<>));

            builder.RegisterGeneric(typeof (NhibernateLinqQueryHandler<>))
                   .As(typeof (IQueryHandler<>));
            builder.RegisterGeneric(typeof(NhibernateStoredProcedureQueryHandler<>))
                   .As(typeof(IQueryHandler<>));
        }

        private static ISessionFactory Initialize(string connectionString)
        {
            return Fluently
                .Configure()
                .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey(connectionString)))
                .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<CategoryTopicEntity>();
                        m.FluentMappings.Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never());
                        m.FluentMappings.Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultCascade.None());
                    })
                .CurrentSessionContext<LazySessionContext>()
                .BuildConfiguration()
                .BuildSessionFactory();
        }
    }
}