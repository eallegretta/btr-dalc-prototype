using System.Linq;
using Autofac;
using BlogTalkRadio.Common.Data.DependencyInjection;
using Cinchcast.Framework.DependencyInjection.Autofac;
using DapperExtensions.Mapper;

namespace BlogTalkRadio.Common.Data.Dapper.DependencyInjection
{
    public class DapperModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DapperConnection>().As<IDapperConnection>();

            builder.RegisterAssemblyTypes(Ioc.Instance.GetAssemblies())
                   .Where(t =>
                       {
                           return t.BaseType.IsGenericType &&
                                  t.BaseType.GetGenericTypeDefinition() == typeof (ClassMapper<>);
                       }).SingleInstance();

            RepositoryModule.RegisterRepository(builder, typeof(DapperRepository<>));
        }
    }
}
