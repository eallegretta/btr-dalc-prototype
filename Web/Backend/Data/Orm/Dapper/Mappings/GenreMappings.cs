using DapperExtensions.Mapper;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Data.Orm.Dapper.Mappings
{
    public class GenreMappings: ClassMapper<GenreEntity>
    {
        public GenreMappings()
        {
            Table("Genres");

            Map(x => x.GenreId).Key(KeyType.Assigned);

            Map(x => x.GenreId).Column("Genre_Id");
            Map(x => x.GenreDescription).Column("Genre_Description");
            Map(x => x.GenreUrl).Column("Genre_URL");
        }
    }
}