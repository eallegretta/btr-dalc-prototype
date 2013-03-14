using System.Linq;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Queries
{
    public abstract class LinqQuery<T> : IQuery
    {
        public abstract void Apply(IQueryable<T> queryable);
    }
}