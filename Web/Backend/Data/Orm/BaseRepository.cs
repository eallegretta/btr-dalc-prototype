using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Orm
{
    public abstract class BaseRepository<T>: IRepository<T> where T: class, new()
    {
        private readonly IEnumerable<IQueryHandler<T>> _queryHandlers;

        public BaseRepository(IEnumerable<IQueryHandler<T>> queryHandlers)
        {
            _queryHandlers = queryHandlers;
        }

        protected virtual IQueryHandler<T> GetQueryHandler(IQuery<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query", "The query is required");
            }

            var queryHandler = _queryHandlers.FirstOrDefault(x => x.CanHandle(query));

            if (queryHandler == null)
            {
                throw new Exception(string.Format("There query of type {0} cannot be handled", query.GetType()));
            }

            return queryHandler;
        }

        public abstract T Get(object id);

        public virtual T Get(IQuery<T> query)
        {
            return GetQueryHandler(query).Get(query);
        }

        public virtual int Count(IQuery<T> query = null)
        {
            if (query == null)
            {
                query = new LinqAdHocQuery<T>();;
            }

            return GetQueryHandler(query).Count(query);
        }

        public virtual List<T> Query(IQuery<T> query)
        {
            return GetQueryHandler(query).Query(query);
        }

        public abstract T SaveOrUpdate(T instance);

        public abstract void Delete(object id);

        public abstract void DeleteAll();
    }
}