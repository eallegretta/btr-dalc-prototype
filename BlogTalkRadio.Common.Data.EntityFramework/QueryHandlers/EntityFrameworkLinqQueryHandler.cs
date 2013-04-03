using System.Collections.Generic;
using System.Linq;
using BlogTalkRadio.Common.Data.EntityFramework;
using BlogTalkRadio.Common.Data.Queries;

namespace BlogTalkRadio.Common.Data.Orm.EntityFramework.QueryHandlers
{
    public class EntityFrameworkLinqQueryHandler<T>: BaseQueryHandler<T> where T: class, new()
    {
        private readonly IDbContextFactorySelector _dbContextFactorySelector;

        public EntityFrameworkLinqQueryHandler(IDbContextFactorySelector dbContextFactorySelector)
        {
            _dbContextFactorySelector = dbContextFactorySelector;
        }

        public override bool CanHandle(IQuery<T> query)
        {
            return query is LinqQuery<T>;
        }

        public override int Count(IQuery<T> query = null)
        {
            return ApplyQuery(query).Count();
        }

        public override List<T> Query(IQuery<T> query)
        {
            return ApplyQuery(query).ToList();
        }

        public override T Get(IQuery<T> query)
        {
            return ApplyQuery(query).FirstOrDefault();
        }

        private IQueryable<T> ApplyQuery(IQuery<T> query)
        {
            var linqQuery = query as LinqQuery<T>;

            var queryable = (IQueryable<T>)_dbContextFactorySelector.GetDbContextFactoryFor(GetDataSourceForQuery(query)).GetCurrentDbContext().Set<T>();

            queryable =  linqQuery.Apply(queryable);
            return queryable;
        }
    }
}