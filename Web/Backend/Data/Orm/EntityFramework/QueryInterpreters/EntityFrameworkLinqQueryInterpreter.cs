using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Orm.EntityFramework.QueryInterpreters
{
    public class EntityFrameworkLinqQueryInterpreter<T>: IQueryInterpreter<T> where T: class, new()
    {
        private readonly IDbContextFactory _dbContextFactory;

        public EntityFrameworkLinqQueryInterpreter(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public bool CanInterpret(IQuery query)
        {
            return query is LinqQuery<T>;
        }

        public int Count(IQuery query = null)
        {
            return ApplyQuery(query).Count();
        }

        public List<T> Query(IQuery query)
        {
            return ApplyQuery(query).ToList();
        }

        public T Get(IQuery query)
        {
            return ApplyQuery(query).FirstOrDefault();
        }

        private IQueryable<T> ApplyQuery(IQuery query)
        {
            var linqQuery = query as LinqQuery<T>;

            var queryable = (IQueryable<T>)_dbContextFactory.GetCurrentDbContext().Set<T>();

            queryable =  linqQuery.Apply(queryable);
            return queryable;
        }
    }
}