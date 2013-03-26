using Autofac;
using Web.Backend.Data;

namespace Web.Backend.DependencyInjection
{
    public class RepositoryFactoryModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof (RepositoryFactoryImpl<>)).As(typeof(IRepositoryFactory<>));
        }
    }
}