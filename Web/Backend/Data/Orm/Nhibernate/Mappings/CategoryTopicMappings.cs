using FluentNHibernate.Mapping;
using Web.Backend.DomainModel.Entities;

namespace BlogTalkRadio.Common.Data.NHibernate.Mappings
{
    public class CategoryTopicMappings : ClassMap<CategoryTopicEntity>
    {
        public CategoryTopicMappings()
        {
            Table("Seo_Topic_Pages");

            Id(x => x.Id);

            References(x => x.Category).Column("Genre_ID").Fetch.Join();

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