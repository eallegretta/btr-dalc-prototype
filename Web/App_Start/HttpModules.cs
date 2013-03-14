using Cinchcast.Framework.Configuration;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Web.App_Start;
using Web.Backend;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof(HttpModules), "Start")]
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