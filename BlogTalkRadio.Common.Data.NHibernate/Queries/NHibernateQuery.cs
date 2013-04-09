using System.Collections.Generic;
using NHibernate;

namespace BlogTalkRadio.Common.Data.NHibernate.Queries
{
    public abstract class NHibernateQuery<T>: IQuery<T> where T: class, new()
    {
        public virtual DataSource DataSource
        {
            get { return null; }
        }

        public abstract IQueryOver<T, T> CreateQuery(IQueryOver<T, T> queryOver);
    }
}
