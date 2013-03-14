using System.Collections.Generic;

namespace Web.Backend.DomainModel.Contracts
{
    public interface IQueryInterpreter<T> where T: class, new()
    {
        bool CanInterpret(IQuery query);
        int Count(IQuery query = null);
        T Get(IQuery query);
        List<T> Query(IQuery query);
    }
}
