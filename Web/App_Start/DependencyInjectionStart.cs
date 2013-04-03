using System;
using System.Linq;
using BlogTalkRadio.Common.Data.NHibernate.DependencyInjection;
using Cinchcast.Framework.DependencyInjection.Autofac;
using Web.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(DependencyInjectionStart), "Start")]
namespace Web.App_Start
{
    public static class DependencyInjectionStart
    {

        public static void Start()
        {
            Ioc.Instance.SetAssembliesResolver(() => AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith("Web", StringComparison.OrdinalIgnoreCase)));
            Ioc.Instance.Initialize(typeof(DependencyInjectionStart).Assembly, typeof(NHibernateModule).Assembly);
        }

    }
}