using Autofac;
using Web.Backend.Contracts;
using Web.Backend.EntityFramework.Mappings;

namespace Web.Backend.EntityFramework.DependencyInjection
{
    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new DbContextFactory {ConnectionStringName = "Default"})
                   .As<IDbContextFactory>()
                   .SingleInstance();

            builder.RegisterType<EntityFrameworkUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<EntityFrameworkQueraybleEagerLoadProvider>().As<IQueryableEagerLoadProvider>();
            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>)).As(typeof(IRepository<>)); //.InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                   .Where(t => t.IsAssignableTo<IEntityMapping>())
                   .As<IEntityMapping>();
        }
    }
}