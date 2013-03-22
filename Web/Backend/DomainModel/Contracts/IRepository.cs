using System.Collections.Generic;

namespace Web.Backend.DomainModel.Contracts
{
    public interface IRepository<T> where T: class, new()
    {
        T Get(object id);
        T Get(IQuery<T> query);
        int Count(IQuery<T> query = null);
        List<T> All(int skip = 0, int take = 1000);
        List<T> Query(IQuery<T> query);
        T SaveOrUpdate(T instance);
        void Delete(object id);
        void DeleteAll();
    }
}
