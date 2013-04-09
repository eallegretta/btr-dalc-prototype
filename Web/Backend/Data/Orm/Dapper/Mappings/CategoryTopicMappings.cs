
using DapperExtensions.Mapper;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Data.Orm.Dapper.Mappings
{
    public class CategoryTopicMappings : ClassMapper<CategoryTopicEntity>
    {
        public CategoryTopicMappings()
        {
            Table("Seo_Topic_Pages");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.MetaDescription);
            Map(x => x.MetaKeywords);
            Map(x => x.MetaTitle);
            Map(x => x.SearchQuery);
            Map(x => x.Url);
        }
    }
}