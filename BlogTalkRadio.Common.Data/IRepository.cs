using System.Collections.Generic;

namespace BlogTalkRadio.Common.Data
{
    public interface IRepository<T> where T: class, new()
    {
        T Get(object id);
        T Get(IQuery<T> query);
        int Count(IQuery<T> query = null);
        List<T> Query(IQuery<T> query);
        T SaveOrUpdate(T instance);
        void Delete(object id);
        void DeleteAll();
    }
}
