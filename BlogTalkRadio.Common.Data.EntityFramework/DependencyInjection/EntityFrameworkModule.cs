using System;
using System.Configuration;
using Autofac;
using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.EntityFramework;
using BlogTalkRadio.Common.Data.Orm.EntityFramework;
using BlogTalkRadio.Common.Data.Orm.EntityFramework.QueryHandlers;

namespace Web.Backend.DependencyInjection.EntityFramework
{
    public class EntityFrameworkModule : Module
    {
        private bool IsQueryHandler(Type type)
        {
            string @namespace = typeof(EntityFrameworkLinqQueryHandler<>).Namespace;

            return type.Namespace == @namespace;
        }

        protected override void Load(ContainerBuilder builder)
        {
            for (int index = 0; index < ConfigurationManager.ConnectionStrings.Count; index++)
            {
                var connectionString = ConfigurationManager.ConnectionStrings[index];
                string name = connectionString.Name;

                string dbContextFactoryKey = name + "-ef-dbContextFactory";
                builder.RegisterInstance(new DbContextFactory { ConnectionStringName = name }).Named<IDbContextFactory>(dbContextFactoryKey).SingleInstance();
            }

            builder.RegisterType<DbContextFactorySelector>().As<IDbContextFactorySelector>();

            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                   .Where(IsQueryHandler)
                   .As(typeof(IQueryHandler<>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
                   .As(typeof(IRepository<>))
                   .SingleInstance();

            builder.RegisterType<EntityFrameworkUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<EntityFrameworkQueraybleEagerLoadProvider>().As<IQueryableEagerLoadProvider>();
        }
    }
}