using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Autofac;
using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.DependencyInjection;
using BlogTalkRadio.Common.Data.EntityFramework;
using BlogTalkRadio.Common.Data.Orm.EntityFramework;
using BlogTalkRadio.Common.Data.Orm.EntityFramework.QueryHandlers;

namespace Web.Backend.DependencyInjection.EntityFramework
{
    public class EntityFrameworkModule : Module
    {
        public const string DBCONTEXT_FACTORY_KEY = "{0}-ef-dbContextFactory";

        protected override void Load(ContainerBuilder builder)
        {
            for (int index = 0; index < ConfigurationManager.ConnectionStrings.Count; index++)
            {
                var connectionString = ConfigurationManager.ConnectionStrings[index];
                string name = connectionString.Name;

                string dbContextFactoryKey = string.Format(DBCONTEXT_FACTORY_KEY, name);
                builder.RegisterInstance(new DbContextFactory { ConnectionStringName = name }).Named<IDbContextFactory>(dbContextFactoryKey).SingleInstance();
            }

            builder.RegisterType<DbContextFactorySelector>().As<IDbContextFactorySelector>();

            RepositoryModule.RegisterRepository(builder, typeof(EntityFrameworkRepository<>));

            builder.RegisterType<EntityFrameworkUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<EntityFrameworkQueraybleEagerLoadProvider>().As<IQueryableEagerLoadProvider>();
        }
    }
}