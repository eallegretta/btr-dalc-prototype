using Web.App_Start;
using Web.Backend.Data;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DataMappingsStart), "Start", Order = 1)]
namespace Web.App_Start
{
    public static class DataMappingsStart
    {
        public static void Start()
        {
            DataSources.InitializeMappings();
        }
    }
}