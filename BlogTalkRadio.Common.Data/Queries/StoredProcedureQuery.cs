using System.Collections.Generic;

namespace BlogTalkRadio.Common.Data.Queries
{
    public abstract class StoredProcedureQuery<T> : IQuery<T> where T: class, new()
    {
        public abstract string StoredProcedure { get; }
        public abstract IDictionary<string, object> Parameters { get; }

        public virtual DataSource DataSource
        {
            get { return null; }
        }
    }
}