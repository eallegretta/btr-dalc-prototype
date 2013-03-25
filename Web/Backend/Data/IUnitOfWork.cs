
namespace Web.Backend.Data
{
    public interface IUnitOfWork
    {
        void Begin();
        void End();
    }
}