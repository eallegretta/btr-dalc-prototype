[assembly: WebActivator.PreApplicationStartMethod(typeof(Web.App_Start.EntityFrameworkProfilerBootstrapper), "PreStart")]
namespace Web.App_Start
{
	public static class EntityFrameworkProfilerBootstrapper
	{
		public static void PreStart()
		{
			// Initialize the profiler
			HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
		}
	}
}

