
namespace Web.Backend.DomainModel.Contracts
{
    public interface IUnitOfWork
    {
        void Begin();
        void End();
    }
}