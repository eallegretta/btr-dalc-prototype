
namespace Web.Backend.Services.Commands
{
    public abstract class CrudDeleteCommand<T> : CrudUpdateCommand<T> where T : class, new()
    {
        public override void Execute()
        {
            if (!IsValid)
                return;

            Repository.Delete(Instance);
        }
    }
}