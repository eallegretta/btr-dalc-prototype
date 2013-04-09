using FluentNHibernate.Mapping;
using Web.Backend.DomainModel.Entities;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Orm.NHibernate.Mappings
{
    public class GenreVersusMappings: ClassMap<GenreEntityNHibernate>
    {
        public GenreVersusMappings()
        {
            Table("Genres");

            Id(x => x.GenreId).Column("Genre_ID");
            Map(x => x.GenreDescription).Column("Genre_Description");
            Map(x => x.GenreUrl).Column("Genre_URL");
        }
    }
}