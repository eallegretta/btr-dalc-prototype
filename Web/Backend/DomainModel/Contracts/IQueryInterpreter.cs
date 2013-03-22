using System.Collections.Generic;

namespace Web.Backend.DomainModel.Contracts
{
    public interface IQueryInterpreter<T> where T: class, new()
    {
        bool CanInterpret(IQuery<T> query);
        int Count(IQuery<T> query = null);
        T Get(IQuery<T> query);
        List<T> Query(IQuery<T> query);
    }
}
