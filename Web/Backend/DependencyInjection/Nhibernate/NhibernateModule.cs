using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Autofac;
using Autofac.Core;
using Cinchcast.Framework.DependencyInjection.Autofac;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Bytecode;
using Web.Backend.Data;
using Web.Backend.Data.Orm;
using Web.Backend.Data.Orm.Nhibernate;

namespace Web.Backend.DependencyInjection.Nhibernate
{
    public class NhibernateModule : Module
    {
        private static readonly Type[] _mappingTypes = GetMappingTypes();
        private static readonly Type[] _queryHandlers = GetQueryHandlers();

        private static Type[] GetQueryHandlers()
        {
            var query = from assembly in Ioc.Instance.GetAssemblies()
                        from type in assembly.GetTypes()
                        let firstInterface = type.GetInterfaces().FirstOrDefault()
                        where !string.IsNullOrWhiteSpace(type.Namespace) 
                                && type.Namespace.EndsWith("Nhibernate.QueryHandlers")
                                && firstInterface != null 
                                && firstInterface.GetGenericTypeDefinition() == typeof(IQueryHandler<>)
                        select type;

            return query.ToArray();
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
                string queryHandlerKey = name + "-queryHandler-{0}";

                builder.Register(x => Initialize(name)).Named<ISessionFactory>(sessionFactoryKey).SingleInstance();

                foreach (var queryHandler in _queryHandlers)
                {
                    builder.RegisterGeneric(queryHandler)
                           .Named(string.Format(queryHandlerKey, queryHandler.Name), typeof (IQueryHandler<>))
                           .WithParameter(ResolvedParameter.ForNamed<ISessionFactory>(sessionFactoryKey)).SingleInstance();
                }

                var registration = builder.RegisterGeneric(typeof (NhibernateRepository<>))
                       .Named(name, typeof (IRepository<>))
                       .WithParameters(new[]
                           {
                               ResolvedParameter.ForNamed<ISessionFactory>(sessionFactoryKey),
                               new ResolvedParameter(
                                   (p, c) =>
                                       {
                                           var type = p.ParameterType.GetGenericArguments().FirstOrDefault();

                                           return type != null && type.GetGenericTypeDefinition() == typeof(IQueryHandler<>);
                                       },
                                   (p, c) =>
                                       {
                                           var queryHandlerList = new ArrayList();

                                           var queryHandlerType = p.ParameterType.GetGenericArguments().First();

                                           foreach (var queryHandler in _queryHandlers)
                                           {
                                               queryHandlerList.Add(c.ResolveNamed(string.Format(queryHandlerKey, queryHandler.Name), queryHandlerType));

                                           }

                                           return queryHandlerList.ToArray(queryHandlerType);
                                       })
                           }
                    ).SingleInstance();

                if (index == 0)
                {
                    registration.As(typeof (IRepository<>));
                }
            }

            builder.RegisterType<NhibernateUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<NhibernateQueryableEagerLoadProvider>().As<IQueryableEagerLoadProvider>();
            
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

        private static IEnumerable<Type> GetMappingTypesForDatabase(string connectionStringName)
        {
            return _mappingTypes.Where(x =>
                {
                    var dbMappingAttribute =
                        x.GetCustomAttributes(typeof(DatabaseMappingAttribute), true)
                         .Cast<DatabaseMappingAttribute>()
                         .FirstOrDefault();

                    if (dbMappingAttribute == null)
                    {
                        return true;
                    }

                    return dbMappingAttribute.ConnectionStringNames.Contains(connectionStringName, StringComparer.OrdinalIgnoreCase);
                });
        }
    }
}