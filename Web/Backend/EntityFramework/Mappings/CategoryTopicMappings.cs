using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using Web.Backend.DomainModel;

namespace Web.Backend.EntityFramework.Mappings
{
    public class CategoryTopicMappings : EntityTypeConfiguration<CategoryTopicEntity>, IEntityMapping
    {
        public CategoryTopicMappings()
        {
            ToTable("Seo_Topic_Pages");

            HasKey(x => x.Id);

            HasRequired(x => x.Category)
                .WithMany(x => x.CategoryTopics)
                .Map(x => x.MapKey("Genre_ID"));

            Property(x => x.Name);
            Property(x => x.Description);
            Property(x => x.MetaDescription);
            Property(x => x.MetaKeywords);
            Property(x => x.MetaTitle);
            Property(x => x.SearchQuery);
            Property(x => x.Url);
        }

        public void Apply(ConfigurationRegistrar configuration)
        {
            configuration.Add(this);
        }
    }
}