using System.Data.Entity.ModelConfiguration.Configuration;

namespace BlogTalkRadio.Common.Data.Orm.EntityFramework.Mappings
{
    public interface IEntityMapping
    {
        void Apply(ConfigurationRegistrar configuration);
    }
}
