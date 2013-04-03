using Web.App_Start;
using Web.Backend.Data;

[assembly: WebActivator.PreApplicationStartMethod(typeof(DataMappingsStart), "Start")]
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