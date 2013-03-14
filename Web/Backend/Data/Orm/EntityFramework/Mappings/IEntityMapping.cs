using System.Data.Entity.ModelConfiguration.Configuration;

namespace Web.Backend.Data.Orm.EntityFramework.Mappings
{
    internal interface IEntityMapping
    {
        void Apply(ConfigurationRegistrar configuration);
    }
}
