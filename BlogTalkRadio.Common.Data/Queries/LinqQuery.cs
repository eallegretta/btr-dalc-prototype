using System.Linq;

namespace BlogTalkRadio.Common.Data.Queries
{
    public abstract class LinqQuery<T> : IQuery<T> where T: class, new()
    {
        public abstract IQueryable<T> Apply(IQueryable<T> queryable);

        public virtual DataSource DataSource
        {
            get { return null; }
        }
    }
}