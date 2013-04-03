
using System.Linq;
using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.FluentMapping;
using BlogTalkRadio.Common.Data.NHibernate.DependencyInjection;
using BlogTalkRadio.Common.Data.NHibernate.Mappings;
using FluentNHibernate;
using FluentNHibernate.Mapping;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Web.App_Start.NHibernateProfilerBootstrapper), "PreStart")]
namespace Web.App_Start
{
	public static class NHibernateProfilerBootstrapper
	{
		public static void PreStart()
		{
			// Initialize the profiler
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
			// You can also use the profiler in an offline manner.
			// This will generate a file with a snapshot of all the NHibernate activity in the application,
			// which you can use for later analysis by loading the file into the profiler.
			// var filename = @"c:\profiler-log";
			// NHibernateProfiler.InitializeOfflineProfiling(filename);

            NHibernateModule.SetMappingTypesForDatabaseEvaluator(
                (connectionStringName, type) =>
                    {
                        if (type.Assembly != typeof(CategoryTopicMappings).Assembly)
                        {
                            return false;
                        }

                        var mappedType = type.BaseType.GetGenericArguments().First();

                        var dataSources = DataSourceMapper.GetDataSourcesForType(mappedType);

                        return dataSources.Any(x => string.Equals(connectionStringName, x.Identifier) && x is SqlDataSource);
                    });
		}
	}
}

