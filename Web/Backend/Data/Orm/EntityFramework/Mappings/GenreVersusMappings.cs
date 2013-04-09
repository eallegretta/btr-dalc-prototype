using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using BlogTalkRadio.Common.Data.Orm.EntityFramework.Mappings;
using Web.Backend.DomainModel.Entities;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Orm.EntityFramework.Mappings
{
    public class GenreVersusMappings : EntityTypeConfiguration<GenreEntityEntityFramework>, IEntityMapping
    {
        public GenreVersusMappings()
        {
            ToTable("Genres");

            HasKey(x => x.GenreId);

            Property(x => x.GenreId).HasColumnName("Genre_Id");
            Property(x => x.GenreDescription).HasColumnName("Genre_Description");
            Property(x => x.GenreUrl).HasColumnName("Genre_URL");
        }

        public void Apply(ConfigurationRegistrar configuration)
        {
            configuration.Add(this);
        }
    }
}