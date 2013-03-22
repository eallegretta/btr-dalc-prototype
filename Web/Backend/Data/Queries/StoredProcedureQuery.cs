using System.Collections.Generic;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Queries
{
    public abstract class StoredProcedureQuery<T> : IQuery<T> where T: class, new()
    {
        public abstract string StoredProcedure { get; }
        public abstract IDictionary<string, object> Parameters { get; }
    }
}