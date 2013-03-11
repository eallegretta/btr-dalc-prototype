
namespace Web.Backend.Contracts
{
    public interface IUnitOfWork
    {
        void Begin();
        void End();
    }
}