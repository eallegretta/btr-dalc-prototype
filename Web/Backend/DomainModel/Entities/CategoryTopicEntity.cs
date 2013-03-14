namespace Web.Backend.DomainModel.Entities
{
    public class CategoryTopicEntity
    {
        public virtual int Id { get; set; }

        private int CategoryId { get; set; }

        public virtual GenreEntity Category { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string MetaTitle { get; set; }
        public virtual string MetaKeywords { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string SearchQuery { get; set; }
        public virtual string Url { get; set; }
        public virtual string RootRelativeUrl
        {
            get { return "/" + Url + "/" + Category.GenreUrl; }
        }
    }
}
