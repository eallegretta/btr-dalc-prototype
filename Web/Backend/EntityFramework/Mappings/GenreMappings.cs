using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using Web.Backend.DomainModel;

namespace Web.Backend.EntityFramework.Mappings
{
    public class GenreMappings: EntityTypeConfiguration<GenreEntity>, IEntityMapping
    {
        public GenreMappings()
        {
            ToTable("Genres");

            HasKey(x => x.GenreId);

            Property(x => x.GenreId).HasColumnName("Genre_Id");
            Property(x => x.GenreDescription).HasColumnName("Genre_Description");
            Property(x => x.GenreUrl).HasColumnName("Genre_URL");

            HasOptional(x => x.ParentGenre)
                .WithMany(x => x.ChildGenres)
                .Map(x => x.MapKey("Parent_Genre"));

            //HasRequired(x => x.ParentGenre)
            //    .WithOptional()
            //    .Map(x => x.MapKey("Parent_Genre"));

            Ignore(x => x.HostCount); //.Formula("(select count(*) from Hosts h where h.Default_Genre_ID = Genre_Id)");
            Ignore(x => x.ShowCount); //.Formula("(select count(*) from Show h where h.Genre_ID = Genre_Id)");
        }

        public void Apply(ConfigurationRegistrar configuration)
        {
            configuration.Add(this);
        }
    }
}