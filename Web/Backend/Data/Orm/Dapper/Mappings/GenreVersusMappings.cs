using DapperExtensions.Mapper;
using Web.Backend.DomainModel.Entities;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Orm.Dapper.Mappings
{
    public class GenreVersusMappings: ClassMapper<GenreEntityDapper>
    {
        public GenreVersusMappings()
        {
            Table("Genres");

            Map(x => x.GenreId).Column("Genre_Id").Key(KeyType.Assigned);
            Map(x => x.GenreDescription).Column("Genre_Description");
            Map(x => x.GenreUrl).Column("Genre_URL");
        }
    }
}