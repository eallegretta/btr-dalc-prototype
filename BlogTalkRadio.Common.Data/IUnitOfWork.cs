
namespace BlogTalkRadio.Common.Data
{
    public interface IUnitOfWork
    {
        void Begin();
        void End();
    }
}