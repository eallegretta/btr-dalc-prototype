using System.Collections.Generic;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Queries
{
    public abstract class StoredProcedureQuery : IQuery
    {
        public abstract string StoredProcedure { get; }
        public abstract IDictionary<string, object> Parameters { get; }
    }
}