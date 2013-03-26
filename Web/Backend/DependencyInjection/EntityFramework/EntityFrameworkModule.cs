using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using Autofac;
using Autofac.Core;
using Cinchcast.Framework.DependencyInjection.Autofac;
using Web.Backend.Data;
using Web.Backend.Data.Orm.EntityFramework;
using Web.Backend.Data.Orm.EntityFramework.Mappings;

namespace Web.Backend.DependencyInjection.EntityFramework
{
    public class EntityFrameworkModule : Module
    {
        private static readonly Type[] _queryHandlers = GetQueryHandlers();

        private static Type[] GetQueryHandlers()
        {
            var query = from assembly in Ioc.Instance.GetAssemblies()
                        from type in assembly.GetTypes()
                        let firstInterface = type.GetInterfaces().FirstOrDefault()
                        where !string.IsNullOrWhiteSpace(type.Namespace)
                                && type.Namespace.EndsWith("EntityFramework.QueryHandlers")
                                && firstInterface != null
                                && firstInterface.GetGenericTypeDefinition() == typeof(IQueryHandler<>)
                        select type;

            return query.ToArray();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                   .Where(t => t.IsAssignableTo<IEntityMapping>())
                   .As<IEntityMapping>();

            for (int index = 0; index < ConfigurationManager.ConnectionStrings.Count; index++)
            {
                var connectionString = ConfigurationManager.ConnectionStrings[index];
                string name = connectionString.Name;

                string dbContextFactoryKey = name + "-ef-dbContextFactory";
                string queryHandlerKey = name + "-ef-queryHandler-{0}";

                builder.RegisterInstance(new DbContextFactory { ConnectionStringName = name }).Named<IDbContextFactory>(dbContextFactoryKey).SingleInstance();

                foreach (var queryHandler in _queryHandlers)
                {
                    builder.RegisterGeneric(queryHandler)
                           .Named(string.Format(queryHandlerKey, queryHandler.Name), typeof(IQueryHandler<>))
                           .WithParameter(ResolvedParameter.ForNamed<IDbContextFactory>(dbContextFactoryKey)).SingleInstance();
                }

                var registration = builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
                       .Named(name, typeof(IRepository<>))
                       .WithParameters(new[]
                           {
                               ResolvedParameter.ForNamed<IDbContextFactory>(dbContextFactoryKey),
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
                    registration.As(typeof(IRepository<>));
                }
            }

            builder.RegisterType<EntityFrameworkUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<EntityFrameworkQueraybleEagerLoadProvider>().As<IQueryableEagerLoadProvider>();

        }

        
    }
}