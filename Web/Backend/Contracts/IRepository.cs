using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Web.Backend.Contracts
{
    public interface IRepository<T> where T: class, new()
    {
        T Get(object id);
        T Get(string field, object value);
        T Get(Expression<Func<T, object>> property, object value);
        T Get(IQuery<T> query);
        int Count(IQuery<T> query = null);
        List<T> GetAll(int skip = 0, int take = 1000);
        List<T> NativeQuery(string query, params object[] inputParameters);
        IQueryable<T> Query();
        T SaveOrUpdate(T instance);
        void Delete(object id);
        void DeleteAll();
    }
}
