using Autofac;
using Web.Backend.Data.Orm.EntityFramework;
using Web.Backend.Data.Orm.EntityFramework.Mappings;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.DependencyInjection.EntityFramework
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