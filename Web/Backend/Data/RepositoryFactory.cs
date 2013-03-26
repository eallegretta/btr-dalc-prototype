using Cinchcast.Framework.DependencyInjection.Autofac;
using Autofac;

namespace Web.Backend.Data
{
    public interface IRepositoryFactory<T> where T: class, new()
    {
        IRepository<T> GetDefault();
        IRepository<T> Get(string connectionStringName);
    }

    public class RepositoryFactoryImpl<T> : IRepositoryFactory<T> where T : class, new()
    {
        public RepositoryFactoryImpl()
        {
        }

        public IRepository<T> GetDefault()
        {
            return Ioc.Instance.Container.Resolve<IRepository<T>>();
        }

        public IRepository<T> Get(string connectionStringName)
        {
            return Ioc.Instance.Container.ResolveNamed<IRepository<T>>(connectionStringName);
        }
    }
}
