using System.Collections.Generic;
using DapperExtensions;
namespace BlogTalkRadio.Common.Data.Dapper.Queries
{
    public abstract class DapperQuery<T>: IQuery<T> where T: class, new()
    {
        public virtual DataSource DataSource { get { return null; } }

        public virtual int? Page { get; set; }
        public virtual int? RecordsToTake { get; set; } 

        public abstract IPredicate Predicate { get; }
        public virtual IList<ISort> Sorts { get { return null; } }
    }
}
