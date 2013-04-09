using BlogTalkRadio.Common.Data.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Web.App_Start;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HttpModules), "Start", Order = 5)]
namespace Web.App_Start
{
    public static class HttpModules
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (UnitOfWorkHttpModule));
        }
    }
}