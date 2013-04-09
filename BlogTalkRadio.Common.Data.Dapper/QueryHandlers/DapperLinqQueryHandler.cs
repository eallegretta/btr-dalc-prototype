using System.Linq;
using DapperExtensions;
using SQLinq.Dapper;

namespace BlogTalkRadio.Common.Data.Dapper.QueryHandlers
{
    public class DapperLinqQueryHandler<T> : IQueryHandler<T> where T : class,new()
    {
        private readonly IDapperConnection _dapperConnection;

        public DapperLinqQueryHandler(IDapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
        } 

        public bool CanHandle(IQuery<T> query)
        {
            throw new System.NotImplementedException();
        }

        public int Count(IQuery<T> query = null)
        {
            using (var cn = _dapperConnection.For<T>())
            {
            }

            throw new System.NotImplementedException();
        }

        public T Get(IQuery<T> query)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<T> Query(IQuery<T> query)
        {
            throw new System.NotImplementedException();
        }
    }
}
