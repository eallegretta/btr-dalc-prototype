using System;
using System.Linq;
using BlogTalkRadio.Common.Data.Dapper.DependencyInjection;
using BlogTalkRadio.Common.Data.DependencyInjection;
using BlogTalkRadio.Common.Data.NHibernate.DependencyInjection;
using Cinchcast.Framework.DependencyInjection.Autofac;
using Web.App_Start;
using Web.Backend.DependencyInjection.EntityFramework;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DependencyInjectionStart), "Start", Order = 3)]
namespace Web.App_Start
{
    public static class DependencyInjectionStart
    {

        public static void Start()
        {
            Ioc.Instance.SetAssembliesResolver(() => AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith("Web", StringComparison.OrdinalIgnoreCase)));
            Ioc.Instance.Initialize(typeof(DependencyInjectionStart).Assembly, typeof(RepositoryModule).Assembly, typeof(NHibernateModule).Assembly, typeof(EntityFrameworkModule).Assembly, typeof(DapperModule).Assembly);
        }

    }
}