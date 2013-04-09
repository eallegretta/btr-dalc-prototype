using System.Linq;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.FluentMapping;
using BlogTalkRadio.Common.Data.Orm.EntityFramework;
using Web.Backend.Data.Orm.EntityFramework.Mappings;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web.App_Start.EntityFrameworkProfilerBootstrapper), "PreStart", Order = 4)]
namespace Web.App_Start
{
	public static class EntityFrameworkProfilerBootstrapper
	{
		public static void PreStart()
		{
			// Initialize the profiler
			//HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            BtrDbContext.SetMappingTypesForDatabaseEvaluator(
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

