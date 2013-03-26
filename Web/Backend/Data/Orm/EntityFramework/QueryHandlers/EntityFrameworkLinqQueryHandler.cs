using System.Collections.Generic;
using System.Linq;
using Web.Backend.Data.Queries;

namespace Web.Backend.Data.Orm.EntityFramework.QueryHandlers
{
    public class EntityFrameworkLinqQueryHandler<T>: IQueryHandler<T> where T: class, new()
    {
        private readonly IDbContextFactory _dbContextFactory;

        public EntityFrameworkLinqQueryHandler(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is LinqQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            return ApplyQuery(query).Count();
        }

        public List<T> Query(IQuery<T> query)
        {
            return ApplyQuery(query).ToList();
        }

        public T Get(IQuery<T> query)
        {
            return ApplyQuery(query).FirstOrDefault();
        }

        private IQueryable<T> ApplyQuery(IQuery<T> query)
        {
            var linqQuery = query as LinqQuery<T>;

            var queryable = (IQueryable<T>)_dbContextFactory.GetCurrentDbContext().Set<T>();

            queryable =  linqQuery.Apply(queryable);
            return queryable;
        }
    }
}