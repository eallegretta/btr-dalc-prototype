using System.Data.Entity.ModelConfiguration.Configuration;
namespace Web.Backend.EntityFramework.Mappings
{
    internal interface IEntityMapping
    {
        void Apply(ConfigurationRegistrar configuration);
    }
}
