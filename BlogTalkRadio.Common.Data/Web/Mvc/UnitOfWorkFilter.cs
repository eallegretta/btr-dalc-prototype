using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace BlogTalkRadio.Common.Data.Web.Mvc
{
    public class UnitOfWorkFilter: ActionFilterAttribute
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkFilter(): this(ServiceLocator.Current.GetInstance<IUnitOfWork>())
        {
            
        }

        public UnitOfWorkFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _unitOfWork.Begin();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _unitOfWork.End();
        }
    }
}
