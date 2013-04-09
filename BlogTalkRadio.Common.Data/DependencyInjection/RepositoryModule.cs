using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reflection = System.Reflection;
using Autofac;
using Autofac.Core;
using BlogTalkRadio.Common.Data.FluentMapping;
namespace BlogTalkRadio.Common.Data.DependencyInjection
{
    public class RepositoryModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var repositoryInterface = typeof (IRepository<>);

            foreach (var type in DataSourceMapping.AllMappings.Keys)
            {
                var genericRepository = (from mapping in DataSourceMapping.AllMappings[type]
                            where mapping.RepositoryType != null
                            select mapping.RepositoryType).Distinct().FirstOrDefault();

                if (genericRepository == null)
                {
                    continue;
                }

                var specificRepositoryInteraface = repositoryInterface.MakeGenericType(type);
                var specificRepository = genericRepository.MakeGenericType(type);

                builder.RegisterType(specificRepository)
                    .WithParameter(GetQueryHandlersParameter(genericRepository.Assembly))
                    .As(specificRepositoryInteraface).SingleInstance();
            }

        }

        public static void RegisterRepository(ContainerBuilder builder, Type repositoryType)
        {
            RegisterQueryHandlers(builder, repositoryType.Assembly);

            builder.RegisterGeneric(repositoryType)
                   .WithParameter(GetQueryHandlersParameter(repositoryType.Assembly))
                   .As(typeof(IRepository<>))
                   .SingleInstance();
        }

        private static void RegisterQueryHandlers(ContainerBuilder builder, Reflection.Assembly queryHandlersAssembly)
        {
            foreach (var queryHandler in GetQueryHandlers(queryHandlersAssembly))
            {
                builder.RegisterGeneric(queryHandler).As(queryHandler).SingleInstance();
            }
        }

        private static ResolvedParameter GetQueryHandlersParameter(Reflection.Assembly queryHandlersAssembly)
        {
            return new ResolvedParameter(
                (p, c) =>
                    {
                        var parameterType = p.ParameterType.GetGenericArguments().FirstOrDefault();

                        return parameterType != null &&
                               parameterType.GetGenericTypeDefinition() == typeof (IQueryHandler<>);
                    },
                (p, c) =>
                    {
                        var queryHandlerList = new ArrayList();

                        var queryHandlerType = p.ParameterType.GetGenericArguments().First();
                        var queryHandlerEntityType = queryHandlerType.GetGenericArguments().First();

                        foreach (var queryHandler in GetQueryHandlers(queryHandlersAssembly))
                        {
                            var specificQueryHandler = queryHandler.MakeGenericType(queryHandlerEntityType);

                            queryHandlerList.Add(c.Resolve(specificQueryHandler));
                        }

                        return queryHandlerList.ToArray(queryHandlerType);
                    });
        }

        private static IEnumerable<Type> GetQueryHandlers(Reflection.Assembly assembly)
        {
            return from t in assembly.GetTypes()
                   from i in t.GetInterfaces()
                   where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<>)
                   select t;
        }
    }
}
