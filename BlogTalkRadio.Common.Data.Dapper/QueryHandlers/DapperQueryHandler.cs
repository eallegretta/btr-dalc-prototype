using System.Collections.Generic;
using System.Linq;
using BlogTalkRadio.Common.Data.Dapper.Queries;
using DapperExtensions;

namespace BlogTalkRadio.Common.Data.Dapper.QueryHandlers
{
    public class DapperQueryHandler<T>: IQueryHandler<T> where T: class, new()
    {
        private readonly IDapperConnection _dapperConnection;

        public DapperQueryHandler(IDapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
        }

        public bool CanHandle(IQuery<T> query)
        {
            return query is DapperQuery<T>;
        }

        public int Count(IQuery<T> query = null)
        {
            var dapperQuery = query as DapperQuery<T>;

            using (var cn = _dapperConnection.For(query))
            {
                return cn.Count<T>(dapperQuery.Predicate);
            }
        }

        public T Get(IQuery<T> query)
        {
            var dapperQuery = query as DapperQuery<T>;

            using (var cn = _dapperConnection.For(query))
            {
                return cn.GetPage<T>(dapperQuery.Predicate, null, 1, 1).FirstOrDefault();
            }
        }

        public List<T> Query(IQuery<T> query)
        {
            var dapperQuery = query as DapperQuery<T>;

            using (var cn = _dapperConnection.For(query))
            {
                if (dapperQuery.Page.HasValue || dapperQuery.RecordsToTake.HasValue)
                {
                    dapperQuery.Page = dapperQuery.Page ?? 1;
                    dapperQuery.RecordsToTake = dapperQuery.RecordsToTake ?? 1000;

                    return cn.GetPage<T>(dapperQuery.Predicate, dapperQuery.Sorts, dapperQuery.Page.Value, dapperQuery.RecordsToTake.Value).ToList();
                }

                return cn.GetList<T>(dapperQuery.Predicate, dapperQuery.Sorts).ToList();
            }
        }
    }
}
