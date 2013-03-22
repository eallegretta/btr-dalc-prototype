using System.Linq;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Queries
{
    public abstract class LinqQuery<T> : IQuery<T> where T: class, new()
    {
        public abstract IQueryable<T> Apply(IQueryable<T> queryable);
    }
}