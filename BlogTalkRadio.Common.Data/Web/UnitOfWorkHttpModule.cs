using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace BlogTalkRadio.Common.Data.Web
{
    public class UnitOfWorkHttpModule: IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().Begin();
            context.EndRequest += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().End();
            context.Error += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().End();
        }
    }
}