using FluentNHibernate.Mapping;
using Web.Backend.DomainModel;

namespace Web.Backend.Nhibernate.Mappings
{
    public class GenreMappings: ClassMap<GenreEntity>
    {
        public GenreMappings()
        {
            Table("Genres");

            Id(x => x.GenreId).Column("Genre_Id");
            Map(x => x.GenreDescription).Column("Genre_Description");
            Map(x => x.GenreUrl).Column("Genre_URL");

            HasManyToMany(x => x.ChildGenres)
                .ParentKeyColumn("Parent_Genre")
                .ChildKeyColumn("Genre_ID")
                .Table("Genres")
                .LazyLoad();
            
            //References(x => x.ParentGenre).Column("Parent_Genre");
            Map(x => x.HostCount).Formula("(select count(*) from Hosts h where h.Default_Genre_ID = Genre_Id)");
            Map(x => x.ShowCount).Formula("(select count(*) from Show h where h.Genre_ID = Genre_Id)");
            HasMany(x => x.CategoryTopics)
                .KeyColumn("Genre_ID")
                .LazyLoad();
        }
    }
}