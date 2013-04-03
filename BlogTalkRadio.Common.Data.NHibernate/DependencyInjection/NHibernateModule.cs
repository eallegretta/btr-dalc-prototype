using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Autofac;
using BlogTalkRadio.Common.Data.NHibernate.QueryHandlers;
using Cinchcast.Framework.DependencyInjection.Autofac;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Bytecode;
using Module = Autofac.Module;

namespace BlogTalkRadio.Common.Data.NHibernate.DependencyInjection
{
    public class NHibernateModule : Module
    {
        private static readonly Type[] _mappingTypes = GetMappingTypes();
        private static Func<string, Type, bool> _mappingTypesForDatabaseEvaluator;

        private bool IsQueryHandler(Type type)
        {
            string @namespace = typeof (NHibernateLinqQueryHandler<>).Namespace;

            return type.Namespace == @namespace;
        }

        private static Type[] GetMappingTypes()
        {
            var query = from assembly in Ioc.Instance.GetAssemblies()
                        from type in assembly.GetTypes()
                        where typeof(IMappingProvider).IsAssignableFrom(type)
                        select type;

            return query.ToArray();
        }

        protected override void Load(ContainerBuilder builder)
        {
            for (int index = 0; index < ConfigurationManager.ConnectionStrings.Count; index++)
            {
                var connectionString = ConfigurationManager.ConnectionStrings[index];
                string name = connectionString.Name;

                string sessionFactoryKey = name + "-sessionFactory";
                builder.Register(x => Initialize(name)).Named<ISessionFactory>(sessionFactoryKey).SingleInstance();
            }

            builder.RegisterType<SessionFactorySelector>().As<ISessionFactorySelector>();

            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                   .Where(IsQueryHandler)
                   .As(typeof(IQueryHandler<>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(NHibernateRepository<>))
                   .As(typeof(IRepository<>))
                   .SingleInstance();

            builder.RegisterType<NHibernateUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<NHibernateQueryableEagerLoadProvider>().As<IQueryableEagerLoadProvider>();
        }

        private static ISessionFactory Initialize(string connectionStringName)
        {
            var mappingTypes = GetMappingTypesForDatabase(connectionStringName);

            return Fluently
                .Configure()
                .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringName)))
                .Mappings(m =>
                    {
                        foreach (var mappingType in mappingTypes)
                        {
                            m.FluentMappings.Add(mappingType);
                        }

                        m.FluentMappings.Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never());
                        m.FluentMappings.Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultCascade.None());
                    })
                .CurrentSessionContext<LazySessionContext>()
                .BuildConfiguration()
                .BuildSessionFactory();
        }

        public static void SetMappingTypesForDatabaseEvaluator(Func<string, Type, bool> evaluator)
        {
            _mappingTypesForDatabaseEvaluator = evaluator;
        }

        private static IEnumerable<Type> GetMappingTypesForDatabase(string connectionStringName)
        {
            return _mappingTypes.Where(x => _mappingTypesForDatabaseEvaluator(connectionStringName, x));
        }
    }
}